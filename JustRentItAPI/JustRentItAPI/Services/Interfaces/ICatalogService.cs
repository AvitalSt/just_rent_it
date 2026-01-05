using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface ICatalogService
    {
        void RunUpdateTaskInBackground();
        Task<Response<byte[]>> GenerateCatalogAsync();
        Task<Response> SaveCatalogAsync(byte[] pdf);
        string GetCatalogUrl();
        Task<Response> UpdateAndSaveCatalogAsync();
    }
}
