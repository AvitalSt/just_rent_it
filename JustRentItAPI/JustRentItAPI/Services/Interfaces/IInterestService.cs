using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IInterestService
    {
        Task<Response> AddInterestAsync(CreateInterestDTO dto);
        Task<Response<PagedResultDTO<InterestListDTO>>> GetFilteredAsync(
            string? status = null,
            string? ownerName = null,
            string? userName = null,
            string? dressName = null,
            DateTime? from = null,
            DateTime? to = null,
            int pageNumber = 1,
            int pageSize = 50);
        Task<Response> UpdateRentalStatusAsync(int interestId, string status);
        Task<Response> UpdatePaymentAmountAsync(int interestId, decimal amount);
        Task<Response> UpdateNotesAsync(int interestId, string notes);
        Task<Response> UpdateOwnerCommentAsync(int interestId, string comment);
        Task<Response> UpdateUserCommentAsync(int interestId, string comment);
        Task<Response> SendMessageToOwnerAsync(int interestId);
        Task<Response> SendMessageToUserAsync(int interestId);
        Task<Response> SendPaymentMessageToOwnerAsync(int interestId);
        Task<List<InterestExportDTO>> GetExportListAsync(
            string? status,
            string? ownerName,
            string? userName,
            string? dressName,
            DateTime? from,
            DateTime? to
            );
/*        Task<Response<List<InterestListDTO>>> GetUserInterestsAsync(int userId);
*/    }
}
