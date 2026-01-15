using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface ICatalogService
    {
        void RunUpdateTaskInBackground();
        string GetCatalogUrl();
        Task<Response> UpdateAndSaveCatalogAsync();
    }
}
