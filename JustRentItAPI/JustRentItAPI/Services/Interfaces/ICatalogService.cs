using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<Response<byte[]>> GenerateCatalogAsync();
        Task<Response> SaveCatalogAsync(byte[] pdf);
        string GetCatalogUrl();
    }
}
