using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustRentItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        
        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response>> AddToWishlist([FromBody] int dressId)
        {
            var response = await _wishlistService.AddToWishlistAsync(dressId);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{dressId}")]
        [Authorize]
        public async Task<ActionResult<Response>> RemoveFromWishlist(int dressId)
        {
            var response = await _wishlistService.RemoveFromWishlistAsync(dressId);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Response<List<DressListDTO>>>> GetMyWishlist()
        {
            var response = await _wishlistService.GetUserWishlistAsync();

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("by-ids")]
        public async Task<ActionResult<Response<List<DressListDTO>>>> GetDressesByIds([FromQuery] string ids)
        {
            var response = await _wishlistService.GetDressesByIdsAsync(ids);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }
    }
}
