using JustRentItAPI.Models.Entities;

namespace JustRentItAPI.Repositories.Interfaces
{
    public interface IMonthlySummaryRepository
    {
        Task AddAsync(MonthlySummary summary);
        Task<MonthlySummary?> GetLastAsync();
    }
}
