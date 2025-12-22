using JustRentItAPI.Mappers;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Enums;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using System.Net;

namespace JustRentItAPI.Services.Classes
{
    public class InterestService : IInterestService
    {
        private readonly IInterestRepository _interestRepository;
        private readonly IDressRepository _dressRepository;
        private readonly IUserRepository _userRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMailService _mailService;

        public InterestService(IInterestRepository interestRepository, IDressRepository dressRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IMailService mailService)
        {
            _interestRepository = interestRepository;
            _dressRepository = dressRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
        }

        public async Task<Response> AddInterestAsync(CreateInterestDTO dto)
        {
            try
            {

                int? userId = UserHelper.GetUserId(_httpContextAccessor);
                if (userId == null)
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "User not authenticated",
                        StatusCode = HttpStatusCode.Unauthorized
                    };

                var todayCount = await _interestRepository.CountUserInterestsToday((int)userId);
                if (todayCount >= 2)
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "הגעת למכסת ההתעניינויות היומית.",
                        StatusCode = HttpStatusCode.BadRequest
                    };

                var interest = InterestMapper.FromCreateDto(dto, userId.Value);
                await _interestRepository.AddAsync(interest);

                var dress = await _dressRepository.GetByIdAsync(dto.DressID);
                var owner = await _userRepository.GetUserByIdAsync(dress.UserID);
                var user = await _userRepository.GetUserByIdAsync(userId.Value);

                await _mailService.SendOwnerInterestAsync(
                    owner,
                    user,
                    dress,
                    dto.Message
                );

                await _mailService.SendUserInterestAsync(
                    owner,
                    user,
                    dress
                );
                return new Response
                {
                    IsSuccess = true,
                    Message = "Interest created successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<Response<PagedResultDTO<InterestListDTO>>> GetFilteredAsync(
        string? status = null,
        string? ownerName = null,
        string? userName = null,
        string? dressName = null,
        DateTime? from = null,
        DateTime? to = null,
        int pageNumber = 1,
        int pageSize = 50)
        {
            try
            {
                // הקריאה לריפוזיטורי עושה את כל עבודת ה־LINQ
                var result = await _interestRepository.GetFilteredAsync(
                    status,
                    ownerName,
                    userName,
                    dressName,
                    from,
                    to,
                    pageNumber,
                    pageSize
                );

                var dtoItems = InterestMapper.ToListDTOs(result.Items);

                return new Response<PagedResultDTO<InterestListDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Interests fetched successfully.",
                    Data = new PagedResultDTO<InterestListDTO>
                    {
                        Items = dtoItems,
                        TotalCount = result.TotalCount,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<PagedResultDTO<InterestListDTO>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<Response> UpdateRentalStatusAsync(int interestId, string status)
        {
            try
            {
                var interest = await _interestRepository.GetByIdAsync(interestId);
                if (interest == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Interest not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                interest.Status = Enum.Parse<RentStatus>(status);
                await _interestRepository.UpdateAsync(interest);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Rental status updated successfully",
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response> UpdatePaymentAmountAsync(int interestId, decimal amount)
        {
            try
            {
                var interest = await _interestRepository.GetByIdAsync(interestId);
                if (interest == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Interest not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                interest.PaymentAmount = amount;

                await _interestRepository.UpdateAsync(interest);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Payment amount updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response> UpdateNotesAsync(int interestId, string notes)
        {
            try
            {
                var interest = await _interestRepository.GetByIdAsync(interestId);
                if (interest == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Interest not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                interest.Notes = notes;
                await _interestRepository.UpdateAsync(interest);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Rental status updated successfully",
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response> UpdateOwnerCommentAsync(int interestId, string comment)
        {
            try
            {
                var interest = await _interestRepository.GetByIdAsync(interestId);
                if (interest == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Interest not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                interest.OwnerComment = comment;
                await _interestRepository.UpdateAsync(interest);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Rental status updated successfully",
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {   
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response> UpdateUserCommentAsync(int interestId, string comment)
        {
            try
            {
                var interest = await _interestRepository.GetByIdAsync(interestId);
                if (interest == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Interest not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                interest.UserComment = comment;
                await _interestRepository.UpdateAsync(interest);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Rental status updated successfully",
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response> SendMessageToOwnerAsync(int interestId)
        {
            try
            {
                var interest = await _interestRepository.GetByIdAsync(interestId);
                if (interest == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Interest not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                await _mailService.SendOwnerFollowUpAsync(
                    interest.Dress.User.Email,
                    interest.Dress.User.FirstName,
                    interest.User.FirstName,
                    interest.Dress.Name
                );

                interest.OwnerMailCount++;
                await _interestRepository.UpdateAsync(interest);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Message sent to owner successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Failed to send message: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response> SendMessageToUserAsync(int interestId)
        {
            try
            {
                var interest = await _interestRepository.GetByIdAsync(interestId);
                if (interest == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Interest not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                await _mailService.SendUserFollowUpAsync(
                    interest.User.Email,
                    interest.User.FirstName,
                    interest.Dress.Name
                );

                interest.UserMailCount++;
                await _interestRepository.UpdateAsync(interest);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Message sent to user successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Failed to send message: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<Response> SendPaymentMessageToOwnerAsync(int interestId)
        {
            try
            {
                var interest = await _interestRepository.GetByIdAsync(interestId);
                if (interest == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Interest not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                await _mailService.SendPaymentAsync(
                    interest.Dress.User.Email,
                    interest.Dress.User.FirstName
                );

                return new Response
                {
                    IsSuccess = true,
                    Message = "Payment notification sent successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<List<InterestExportDTO>> GetExportListAsync(
           string? status,
           string? ownerName,
           string? userName,
           string? dressName,
           DateTime? from,
           DateTime? to)
        {
            try
            {
                var response = await GetFilteredAsync(
                    status,
                    ownerName,
                    userName,
                    dressName,
                    from,
                    to,
                    pageNumber: 1,
                    pageSize: int.MaxValue
                );

                if (response == null || !response.IsSuccess || response.Data == null)
                    return new List<InterestExportDTO>();

                var items = response.Data.Items;

                return items.Select(x => new InterestExportDTO
                {
                    CreatedDate = x.SentDate,
                    UserName = x.UserName,
                    OwnerName = x.OwnerName,
                    DressName = x.DressName,
                    Status = Enum.Parse<RentStatus>(x.Status),
                    Price = x.PaymentAmount,
                    Notes = x.Notes,
                    OwnerComment = x.OwnerComment,
                    OwnerEmail = x.OwnerEmail,
                    UserComment = x.UserComment,
                    UserEmail = x.UserEmail
                }).ToList();
            }
            catch
            {
                return new List<InterestExportDTO>();
            }
        }

        /*        public async Task<Response<List<InterestListDTO>>> GetUserInterestsAsync(int userId)
                {
                    try
                    {
                        var interests = await _interestRepository.GetUserInterestsAsync(userId);

                        var interestDTOs = MapToDTOs(interests);

                        return new Response<List<InterestListDTO>>
                        {
                            IsSuccess = true,
                            Message = "User interests retrieved successfully",
                            StatusCode = HttpStatusCode.OK,
                            Data = interestDTOs
                        };
                    }
                    catch (Exception ex)
                    {
                        return new Response<List<InterestListDTO>>
                        {
                            IsSuccess = false,
                            Message = ex.Message,
                            StatusCode = HttpStatusCode.InternalServerError
                        };
                    }
                }
        */
    }
}
