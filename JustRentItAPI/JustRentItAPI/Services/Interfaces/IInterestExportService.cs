using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IInterestExportService
    {
        byte[] GenerateExcel(List<InterestExportDTO> list);
    }
}
