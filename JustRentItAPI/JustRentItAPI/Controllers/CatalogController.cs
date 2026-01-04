using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JustRentItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpPost("update")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateCatalog()
        {
            var response = await _catalogService.UpdateAndSaveCatalogAsync();

            if (response.IsSuccess)
                return Ok(response);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public IActionResult GetCatalog()
        {
            var url = _catalogService.GetCatalogUrl();

            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return Redirect(url);
        }
    }
}
