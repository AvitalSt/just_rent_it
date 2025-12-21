using JustRentItAPI.Mappers;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Models.Enums;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using JustRentItAPI.Utils;
using System.Net;

namespace JustRentItAPI.Services.Classes
{
    public class DressService : IDressService
    {
        private readonly IDressRepository _dressRepository;
        private readonly IWishlistRepository _favoriteRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMailService _mailService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;

        public DressService(IDressRepository dressRepository, IWishlistRepository favoriteRepository, IImageRepository imageRepository, IMailService mailService, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _dressRepository = dressRepository;
            _favoriteRepository = favoriteRepository;
            _imageRepository = imageRepository;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = config["FrontendUrl"];
        }


        private async Task<Dress?> GetDressAsync(int id)
        {
            return await _dressRepository.GetByIdAsync(id);
        }

        public async Task<Response<PagedResultDTO<DressListDTO>>> GetFilteredAsync(
            string? city = null,
            string? eventType = null,
            string? saleType = null,
            string? ageGroup = null,
            string? colorGroup = null,
            string? sizeGroup = null,
            string? priceGroup = null,
            string? stateGroup = null,
            string? statusGroup = null,
            string? orderBy = null,
            int pageNumber = 1,
            int pageSize = 24)
        {
            try
            {
                bool isAdmin = UserHelper.IsAdmin(_httpContextAccessor);
                var favoriteDressIds = await UserHelper.GetUserFavoriteDressIdsAsync(_httpContextAccessor, _favoriteRepository);

                var cityIDs = FilterParser.ParseIds(city);
                var eventTypeIDs = FilterParser.ParseIds(eventType);
                var ageGroupIDs = FilterParser.ParseIds(ageGroup);
                var colorIDs = FilterParser.ParseIds(colorGroup);
                var sizeIDs = FilterParser.ParseIds(sizeGroup);

                var saleTypeEnum = FilterParser.ParseEnum<SaleType>(saleType);
                var stateGroupEnum = FilterParser.ParseEnums<DressState>(stateGroup);
                var statusGroupEnum = FilterParser.ParseEnums<DressStatus>(statusGroup);

                var result = await _dressRepository.GetFilteredAsync(
           cityIDs, eventTypeIDs, saleTypeEnum, ageGroupIDs,
           colorIDs, sizeIDs, priceGroup, stateGroupEnum, statusGroupEnum,
           orderBy, pageNumber, pageSize, isAdmin
       );

                var dtoItems = result.Items
                                     .Select(d => DressMapper.MapToListDTO(d, favoriteDressIds))
                                     .ToList();

                return new Response<PagedResultDTO<DressListDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Dresses fetched successfully.",
                    Data = new PagedResultDTO<DressListDTO>
                    {
                        Items = dtoItems,
                        TotalCount = result.TotalCount,
                        maxPrice = result.maxPrice,

                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<PagedResultDTO<DressListDTO>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<Response<DressDTO>> GetDressByIdAsync(int id)
        {
            bool isAdmin = UserHelper.IsAdmin(_httpContextAccessor);
            try
            {
                var favoriteDressIds = await UserHelper.GetUserFavoriteDressIdsAsync(_httpContextAccessor, _favoriteRepository);
                var dress = await _dressRepository.GetByIdForUpdateAsync(id);

                if (dress == null)
                    return new Response<DressDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Dress not found."
                    };

                if (dress.Status != DressStatus.Active && !isAdmin)
                {
                    return new Response<DressDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.Forbidden,
                        Message = "אין לך הרשאה לצפות בשמלה זו."
                    };
                }
                if (!isAdmin)
                {
                    dress.Views++;
                    await _dressRepository.UpdateAsync(dress);
                }

                var dto = DressMapper.MapToDetailedDTO(dress, favoriteDressIds);

                return new Response<DressDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Dress fetched successfully.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new Response<DressDTO>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<Response<DressDTO>> AddDressAsync(AddDressDTO dto)
        {
            try
            {
                int? userId = UserHelper.GetUserId(_httpContextAccessor);
                if (userId == null)
                    return new Response<DressDTO>
                    {
                        IsSuccess = false,
                        Message = "User not authenticated",
                        StatusCode = HttpStatusCode.Unauthorized
                    };

                var dress = CreateDressFromDto(dto, userId.Value);

                var savedDress = await _dressRepository.AddAsync(dress);

                var userFavoriteDressIds = new List<int>();
                var dressDto = DressMapper.MapToDetailedDTO(savedDress, userFavoriteDressIds);

                return new Response<DressDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Dress added successfully.",
                    Data = dressDto
                };
            }
            catch (Exception ex)
            {
                return new Response<DressDTO>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<Response<DressDTO>> UpdateDressAsync(int id, UpdateDressDTO dto)
        {
            try
            {
                var dress = await _dressRepository.GetByIdForUpdateAsync(id);
                if (dress == null)
                    return new Response<DressDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Dress not found."
                    };

                await ApplyDressUpdateAsync(dress, dto);

                await _dressRepository.UpdateAsync(dress);

                var favs = await UserHelper.GetUserFavoriteDressIdsAsync(_httpContextAccessor, _favoriteRepository);

                return new Response<DressDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Dress updated successfully.",
                    Data = DressMapper.MapToDetailedDTO(dress, favs)
                };
            }
            catch (Exception ex)
            {
                return new Response<DressDTO>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<DressDTO>> DeleteDressAsync(int id)
        {
            try
            {
                var dress = await _dressRepository.GetByIdForUpdateAsync(id);
                if (dress == null)
                    return new Response<DressDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Dress not found."
                    };

                dress.Status = DressStatus.Deleted;
                await _dressRepository.UpdateAsync(dress);
                await _mailService.SendDressDeletedAsync(dress);

                var userFavoriteDressIds = await UserHelper.GetUserFavoriteDressIdsAsync(_httpContextAccessor, _favoriteRepository);
                var dressDto = DressMapper.MapToDetailedDTO(dress, userFavoriteDressIds);

                return new Response<DressDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Dress deleted successfully.",
                    Data = dressDto
                };
            }
            catch (Exception ex)
            {
                return new Response<DressDTO>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<DressDTO>> ActivateDressAsync(int id)
        {
            try
            {
                var dress = await _dressRepository.GetByIdForUpdateAsync(id);
                if (dress == null)
                    return new Response<DressDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Dress not found."
                    };

                dress.Status = DressStatus.Active;
                await _dressRepository.UpdateAsync(dress);
                await _mailService.SendDressActivatedAsync(dress, _baseUrl);

                var userFavoriteDressIds = await UserHelper.GetUserFavoriteDressIdsAsync(_httpContextAccessor, _favoriteRepository);
                var dressDto = DressMapper.MapToDetailedDTO(dress, userFavoriteDressIds);

                return new Response<DressDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Dress activate successfully.",
                    Data = dressDto
                };
            }
            catch (Exception ex)
            {
                return new Response<DressDTO>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<Response<UserDTO>> GetDressOwnerAsync(int dressId)
        {
            try
            {
                var dress = await _dressRepository.GetByIdAsync(dressId);
                if (dress == null)
                {
                    return new Response<UserDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Dress not found."
                    };
                }

                var user = await _dressRepository.GetOwnerByIdAsync(dress.UserID);
                if (user == null)
                {
                    return new Response<UserDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Owner user not found."
                    };
                }

                var dto = AuthMapper.ToUserDTO(user, new List<int>());

                return new Response<UserDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new Response<UserDTO>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<List<DressListDTO>>> GetMostViewedAsync()
        {
            try
            {
                var dresses = await _dressRepository.GetMostViewedAsync();

                var dtoItems = dresses
                                .Select(d => DressMapper.MapToPublicListDTO(d))
                                .ToList();

                return new Response<List<DressListDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Most viewed dresses retrieved successfully.",
                    Data = dtoItems
                };
            }
            catch (Exception ex)
            {
                return new Response<List<DressListDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        private Dress CreateDressFromDto(AddDressDTO dto, int userId)
        {
            var dress = new Dress
            {
                UserID = userId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                SaleType = (SaleType)dto.SaleType,
                State = (DressState)dto.State,
                Status = DressStatus.Pending,
                CreatedDate = DateTime.UtcNow
            };

            dress.MainImage = !string.IsNullOrWhiteSpace(dto.MainImage)
                ? dto.MainImage
                : dto.ImagePaths?.FirstOrDefault();

            foreach (var id in dto.ColorIds)
                dress.Colors.Add(new DressColor { ColorID = id });

            foreach (var id in dto.SizeIds)
                dress.Sizes.Add(new DressSize { SizeID = id });

            foreach (var id in dto.CityIds)
                dress.Cities.Add(new DressCity { CityID = id });

            foreach (var id in dto.AgeGroupIds)
                dress.AgeGroups.Add(new DressAgeGroup { AgeGroupID = id });

            foreach (var id in dto.EventTypeIds)
                dress.EventTypes.Add(new DressEventType { EventTypeID = id });

            if (dto.ImagePaths?.Any() == true)
            {
                foreach (var path in dto.ImagePaths.Distinct())
                {
                    dress.Images.Add(new DressImage
                    {
                        ImagePath = path,
                        IsMain = path == dress.MainImage
                    });
                }
            }

            return dress;
        }

        private async Task ApplyDressUpdateAsync(Dress dress, UpdateDressDTO dto)
        {
            if (dto.Name != null) dress.Name = dto.Name;
            if (dto.Description != null) dress.Description = dto.Description;
            if (dto.Price.HasValue) dress.Price = dto.Price.Value;
            if (dto.SaleType.HasValue) dress.SaleType = dto.SaleType.Value;
            if (dto.State.HasValue) dress.State = dto.State.Value;
            if (dto.Status.HasValue) dress.Status = dto.Status.Value;

            if (dto.ColorIDs != null)
            {
                dress.Colors.Clear();
                foreach (var id in dto.ColorIDs)
                    dress.Colors.Add(new DressColor { DressID = dress.DressID, ColorID = id });
            }

            if (dto.SizeIDs != null)
            {
                dress.Sizes.Clear();
                foreach (var id in dto.SizeIDs)
                    dress.Sizes.Add(new DressSize { DressID = dress.DressID, SizeID = id });
            }

            if (dto.CityIDs != null)
            {
                dress.Cities.Clear();
                foreach (var id in dto.CityIDs)
                    dress.Cities.Add(new DressCity { DressID = dress.DressID, CityID = id });
            }

            if (dto.AgeGroupIDs != null)
            {
                dress.AgeGroups.Clear();
                foreach (var id in dto.AgeGroupIDs)
                    dress.AgeGroups.Add(new DressAgeGroup { DressID = dress.DressID, AgeGroupID = id });
            }

            if (dto.EventTypeIDs != null)
            {
                dress.EventTypes.Clear();
                foreach (var id in dto.EventTypeIDs)
                    dress.EventTypes.Add(new DressEventType { DressID = dress.DressID, EventTypeID = id });
            }

            if (dto.RemoveImageIds?.Any() == true)
            {
                foreach (var imgId in dto.RemoveImageIds)
                {
                    var img = dress.Images.FirstOrDefault(i => i.DressImageID == imgId);
                    if (img != null)
                    {
                        await _imageRepository.DeleteAsync(imgId);
                        dress.Images.Remove(img);
                    }
                }
            }

            if (dto.AddImagePaths?.Any() == true)
            {
                foreach (var path in dto.AddImagePaths)
                {
                    if (string.IsNullOrWhiteSpace(path)) continue;
                    if (dress.Images.Any(i => i.ImagePath == path)) continue;

                    dress.Images.Add(new DressImage
                    {
                        DressID = dress.DressID,
                        ImagePath = path,
                        IsMain = false
                    });
                }
            }

            if (dress.Images.Any())
            {
                foreach (var img in dress.Images)
                    img.IsMain = false;

                var main = !string.IsNullOrWhiteSpace(dto.MainImage)
                    ? dress.Images.FirstOrDefault(i => i.ImagePath == dto.MainImage)
                    : dress.Images.First();

                main.IsMain = true;
                dress.MainImage = main.ImagePath;
            }
        }

        /*        public async Task<Response<List<DressListDTO>>> GetUserDressesAsync()
                {
                    try
                    {
                        int? userId = UserHelper.GetUserId(_httpContextAccessor);
                        if (userId == null)
                            return new Response<List<DressListDTO>>
                            {
                                IsSuccess = false,
                                Message = "User not authenticated",
                                StatusCode = HttpStatusCode.Unauthorized
                            };

                        var favoriteDressIds = await _favoriteRepository.GetUserFavoriteDressIdsAsync((int)userId);
                        var dresses = await _dressRepository.GetByUserIdAsync((int)userId);

                        var dressDtos = dresses.Select(d => DressMapper.MapToListDTO(d, favoriteDressIds)).ToList();

                        return new Response<List<DressListDTO>>
                        {
                            IsSuccess = true,
                            StatusCode = HttpStatusCode.OK,
                            Data = dressDtos
                        };
                    }
                    catch (Exception ex)
                    {
                        return new Response<List<DressListDTO>>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = ex.Message
                        };
                    }
                }
        */
    }
}
