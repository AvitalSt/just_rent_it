using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IMonthlySummaryService
    {
        Task<Response<MonthlySummaryLastDTO>> GetLastSummaryAsync();
        Task<Response<MonthlySummaryPreviewDTO>> PreviewMonthlySummaryAsync();
        Task<Response> SendMonthlySummaryAsync(int daysToSplit, int dayIndex);
    }
}
