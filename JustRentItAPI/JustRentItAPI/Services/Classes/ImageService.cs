using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using System.Net;

namespace JustRentItAPI.Services.Classes
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        private string OptimizeUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return url;
            return url.Replace("/upload/", "/upload/f_auto,q_auto/");
        }

        public async Task<Response<List<DressImageDTO>>> UploadImagesAsync(IFormFileCollection files, int? dressId = null)
        {
            try
            {
                var uploadedImages = new List<DressImageDTO>();

                foreach (var file in files)
                {
                    var image = await _imageRepository.UploadAsync(file, dressId);
                    if (image != null)
                    {
                        image.ImagePath = OptimizeUrl(image.ImagePath);
                        uploadedImages.Add(image);
                    }
                }

                return new Response<List<DressImageDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Image uploaded successfully.",
                    Data = uploadedImages
                };
            }
            catch (Exception ex)
            {
                return new Response<List<DressImageDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Failed to upload image: {ex.Message}"
                };
            }
        }

        public async Task<Response> DeleteImageAsync(int imageId)
        {
            try
            {
                var success = await _imageRepository.DeleteAsync(imageId);
                if (!success)
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Image not found or could not be deleted."
                    };

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Image deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Failed to delete image: {ex.Message}"
                };
            }
        }

       /* public async Task<Response> SetMainImageAsync(int dressId, int imageId)
        {
            try
            {
                var success = await _imageRepository.SetMainImageAsync(dressId, imageId);
                if (!success)
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Image not found or could not be set as main."
                    };

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Main image set successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Failed to set main image: {ex.Message}"
                };
            }
        }

        public async Task<Response<List<DressImageDTO>>> GetImagesByDressAsync(int dressId)
        {
            try
            {
                var images = await _imageRepository.GetImagesByDressIdAsync(dressId);
                return new Response<List<DressImageDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Images fetched successfully.",
                    Data = images
                };
            }
            catch (Exception ex)
            {
                return new Response<List<DressImageDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Failed to fetch images: {ex.Message}"
                };
            }
        }*/
    }
}