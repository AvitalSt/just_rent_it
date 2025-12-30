using JustRentItAPI.Models.DTOs;
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
        public async Task<ActionResult<Response>> UpdateCatalog()
        {
            var pdfResponse = await _catalogService.GenerateCatalogAsync();

            if (!pdfResponse.IsSuccess || pdfResponse.Data == null)
                return StatusCode((int)pdfResponse.StatusCode, pdfResponse);

            var saveResponse = await _catalogService.SaveCatalogAsync(pdfResponse.Data);

            if (saveResponse.IsSuccess)
                return Ok(saveResponse);

            return StatusCode((int)saveResponse.StatusCode, saveResponse);
        }

        [HttpGet]
        public IActionResult GetCatalog()
        {
            var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
            var url = $"https://res.cloudinary.com/{cloudName}/raw/upload/catalog/latest.pdf";

            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return Redirect(url);
        }

        /* [HttpGet]
         public async Task<IActionResult> GetCatalog()
         {
             var pdf = await _catalogService.GetCatalogAsync();
             if (pdf.Length == 0)
                 return NotFound("קטלוג עדיין לא נוצר");

             return File(pdf, "application/pdf", "catalog.pdf");
         }*/

    }
}
