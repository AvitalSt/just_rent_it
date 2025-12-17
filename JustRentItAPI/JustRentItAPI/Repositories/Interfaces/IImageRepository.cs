using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<DressImageDTO> UploadAsync(IFormFile file, int? dressId = null);
        Task<bool> DeleteAsync(int imageId);
       /* Task<bool> SetMainImageAsync(int dressId, int imageId);
        Task<List<DressImageDTO>> GetImagesByDressIdAsync(int dressId);*/

    }
}
