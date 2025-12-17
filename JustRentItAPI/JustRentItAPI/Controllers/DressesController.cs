using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustRentItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DressesController : ControllerBase
    {
        private readonly IDressService _dressService;

        public DressesController(IDressService dressService)
        {
            _dressService = dressService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<PagedResultDTO<DressListDTO>>>> GetFiltered(
        [FromQuery] string? city,
        [FromQuery] string? eventType,
        [FromQuery] string? saleType,
        [FromQuery] string? ageGroup,
        [FromQuery] string? colorGroup,
        [FromQuery] string? sizeGroup,
        [FromQuery] string? priceGroup,
        [FromQuery] string? stateGroup,
        [FromQuery] string? statusGroup,
        [FromQuery] string? orderBy,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 24)
        {
            var response = await _dressService.GetFilteredAsync(
                city, eventType, saleType, ageGroup,
                colorGroup, sizeGroup, priceGroup,
                stateGroup, statusGroup, orderBy,
                pageNumber, pageSize);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<DressDTO>>> GetDressById(int id)
        {
            var response = await _dressService.GetDressByIdAsync(id);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response<DressDTO>>> AddDress([FromBody] AddDressDTO dto)
        {
            var response = await _dressService.AddDressAsync(dto);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<DressDTO>>> UpdateDress(int id, [FromBody] UpdateDressDTO dto)
        {
            var response = await _dressService.UpdateDressAsync(id, dto);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteDress(int id)
        {
            var response = await _dressService.DeleteDressAsync(id);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<DressDTO>>> ActivateDress(int id)
        {
            var response = await _dressService.ActivateDressAsync(id);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("most-viewed")]
        public async Task<ActionResult<Response<List<DressListDTO>>>> GetMostViewed()
        {
            var response = await _dressService.GetMostViewedAsync();

            if (!response.IsSuccess)
                return StatusCode((int)response.StatusCode, response);

            return Ok(response);
        }

        [HttpGet("{id}/owner")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<UserDTO>>> GetDressOwner(int id)
        {
            var response = await _dressService.GetDressOwnerAsync(id);

            if (response.IsSuccess)
                return Ok(response);

            return StatusCode((int)response.StatusCode, response);
        }

        /* [HttpGet("my-dresses")]
         [Authorize]
         public async Task<ActionResult<Response<List<DressListDTO>>>> GetMyDresses()
         {
             var response = await _dressService.GetUserDressesAsync();

             if (response.IsSuccess)
                 return Ok(response);
             else
                 return StatusCode((int)response.StatusCode, response);
         }
        */
    }
}
