using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult UpdateCatalog()
        {
            _catalogService.RunUpdateTaskInBackground();

            return Accepted(new
            {
                IsSuccess = true,
                Message = "תהליך עדכון הקטלוג התחיל ברקע."
            });
        }

        [HttpGet]
        public IActionResult GetCatalog()
        {
            var url = _catalogService.GetCatalogUrl();

            return Redirect(url);
        }
    }
}
