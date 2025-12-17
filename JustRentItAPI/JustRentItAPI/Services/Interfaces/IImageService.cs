using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IImageService
    {
        Task<Response<List<DressImageDTO>>> UploadImagesAsync(IFormFileCollection files, int? dressId = null);
        Task<Response> DeleteImageAsync(int imageId);
      /*  Task<Response> SetMainImageAsync(int dressId, int imageId);
        Task<Response<List<DressImageDTO>>> GetImagesByDressAsync(int dressId);*/
    }
}
