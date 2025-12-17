using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IMonthlySummaryService
    {
        Task<Response> SendMonthlySummaryAsync();
        Task<Response<MonthlySummaryLastDTO>> GetLastSummaryAsync();
    }
}
