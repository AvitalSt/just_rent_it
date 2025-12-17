using JustRentItAPI.Mappers;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using System.Net;

namespace JustRentItAPI.Services.Classes
{
    public class WishlistService : IWishlistService
    {
        private IWishlistRepository _favoriteRepository;
        private readonly IDressRepository _dressRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishlistService(IWishlistRepository favoriteRepository, IDressRepository dressRepository, IHttpContextAccessor httpContextAccessor)
        {
            _favoriteRepository = favoriteRepository;
            _dressRepository = dressRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> AddToWishlistAsync(int dressId)
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

                await _favoriteRepository.AddFavoriteAsync((int)userId, dressId);

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Added to favorites"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }

        }

        public async Task<Response> RemoveFromWishlistAsync(int dressId)
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

                await _favoriteRepository.RemoveFavoriteAsync((int)userId, dressId);

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Removed from favorites"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<List<DressListDTO>>> GetUserWishlistAsync()
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
                
                var dresses = new List<Dress>();

                foreach (var id in favoriteDressIds)
                {
                    var dress = await _dressRepository.GetByIdAsync(id);
                    if (dress != null)
                        dresses.Add(dress);
                }

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

        public async Task<Response<List<DressListDTO>>> GetDressesByIdsAsync(string ids)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ids))
                {
                    return new Response<List<DressListDTO>>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "No IDs provided."
                    };
                }

                var idList = ids.Split(',')
                                .Select(id => int.TryParse(id, out var num) ? num : (int?)null)
                                .Where(id => id.HasValue)
                                .Select(id => id.Value)
                                .ToList();

                var dresses = await _dressRepository.GetByIdsAsync(idList);

                var dtos = dresses.Select(d => DressMapper.MapToListDTO(d, new List<int>()))
                                  .ToList();

                return new Response<List<DressListDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos
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

    }
}
