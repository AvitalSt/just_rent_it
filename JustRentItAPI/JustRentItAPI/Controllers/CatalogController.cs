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
        public IActionResult UpdateCatalog()
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("[Background] Starting catalog update...");
                    await _catalogService.UpdateAndSaveCatalogAsync();
                    Console.WriteLine("[Background] Catalog update finished successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Background] Error: {ex.Message}");
                }
            });

            return Accepted(new
            {
                IsSuccess = true,
                Message = "תהליך עדכון הקטלוג התחיל ברקע. תוכל להוריד את הקטלוג המעודכן בעוד כדקה."
            });
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
