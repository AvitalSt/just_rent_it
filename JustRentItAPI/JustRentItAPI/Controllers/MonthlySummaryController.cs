using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustRentItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlySummaryController : ControllerBase
    {
        private readonly IMonthlySummaryService _monthlySummaryService;

        public MonthlySummaryController(IMonthlySummaryService monthlySummaryService)
        {
            _monthlySummaryService = monthlySummaryService;
        }

        [HttpPost("send")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> SendMonthlySummary()
        {
            var response = await _monthlySummaryService.SendMonthlySummaryAsync();
            if (response.IsSuccess)
                return Ok(response);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("last")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<MonthlySummaryLastDTO>>> GetLastSummary()
        {
            var response = await _monthlySummaryService.GetLastSummaryAsync();
            if (response.IsSuccess)
                return Ok(response);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
