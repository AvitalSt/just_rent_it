using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustRentItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        [Authorize]
        //IFormFileCollection אוסף של קבצים שנקבל
        //בבקשה שמכילה קבצים אין JSON, ולכן שדות נלווים (כמו dressId) חייבים להגיע מ־FormData.
        public async Task<ActionResult<Response<DressImageDTO>>> Upload([FromForm] IFormFileCollection files , [FromForm] int? dressId = null)
        {
            var response= await _imageService.UploadImagesAsync(files, dressId);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{imageId}")]
        [Authorize]
        public async Task<ActionResult<Response>> Delete(int imageId)
        {
            var response = await _imageService.DeleteImageAsync(imageId);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }
    }
}
