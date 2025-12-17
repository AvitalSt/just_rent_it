using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JustRentItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionsService _service;

        public OptionsController(IOptionsService service)
        {
            _service = service;
        }

        [HttpGet("sizes")]
        public async Task<ActionResult<Response<List<SizeDTO>>>> GetSizes()
        {
            var response = await _service.GetSizesAsync();

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("age-groups")]
        public async Task<ActionResult<Response<List<AgeGroupDTO>>>> GetAgeGroups()
        {
            var response = await _service.GetAgeGroupsAsync();

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("cities")]
        public async Task<ActionResult<Response<List<CityDTO>>>> GetCities()
        {
            var response = await _service.GetCitiesAsync();

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("event-types")]
        public async Task<ActionResult<Response<List<EventTypeDTO>>>> GetEventTypes()
        {
            var response = await _service.GetEventTypesAsync();

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("colors")]
        public async Task<ActionResult<Response<List<ColorDTO>>>> GetColors()
        {
            var response = await _service.GetColorsAsync();

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }
    }
}
