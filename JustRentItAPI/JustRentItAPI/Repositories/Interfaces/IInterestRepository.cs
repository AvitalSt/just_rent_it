using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;

namespace JustRentItAPI.Repositories.Interfaces
{
    public interface IInterestRepository
    {
        Task<int> CountUserInterestsToday(int userId);
        Task AddAsync(Interest interest);
        Task<PagedResultDTO<Interest>> GetFilteredAsync(
            string? status = null,
            string? ownerName = null,
            string? userName = null,
            string? dressName = null,
            DateTime? from = null,
            DateTime? to = null,
            int pageNumber = 1,
            int pageSize = 50
        );
        Task<Interest?> GetByIdAsync(int interestId);
        Task UpdateAsync(Interest interest);
        Task<List<Interest>> GetByDateRangeAsync(DateTime from, DateTime to);
/*        Task<List<Interest>> GetUserInterestsAsync(int userId);
*/    }
}
