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
        public IActionResult UpdateCatalog([FromServices] IServiceScopeFactory scopeFactory)
        {
            _ = Task.Run(async () =>
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    try
                    {
                        var scopedCatalogService = scope.ServiceProvider.GetRequiredService<ICatalogService>();

                        Console.WriteLine("[Background] Starting catalog update with a NEW scope...");
                        await scopedCatalogService.UpdateAndSaveCatalogAsync();
                        Console.WriteLine("[Background] Catalog update finished successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Background] FATAL ERROR: {ex.Message}");
                        if (ex.InnerException != null)
                            Console.WriteLine($"Inner: {ex.InnerException.Message}");
                    }
                }
            });

            return Accepted(new
            {
                IsSuccess = true,
                Message = "תהליך עדכון הקטלוג התחיל ברקע בסביבה מבודדת."
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
