using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustRentItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly IInterestService _interestService;
        private readonly IInterestExportService _interestExportService;

        public InterestController(IInterestService interestService, IInterestExportService interestExportService)
        {
            _interestService = interestService;
            _interestExportService = interestExportService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response>> CreateInterest([FromBody] CreateInterestDTO dto)
        {
            var response = await _interestService.AddInterestAsync(dto);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<PagedResultDTO<InterestListDTO>>>> GetFiltered(
          [FromQuery] string? status,
          [FromQuery] string? ownerName,
          [FromQuery] string? userName,
          [FromQuery] string? dressName,
          [FromQuery] DateTime? from,
          [FromQuery] DateTime? to,
          [FromQuery] int pageNumber = 1,
          [FromQuery] int pageSize = 10)
        {
            var response = await _interestService.GetFilteredAsync(
                status,
                ownerName,
                userName,
                dressName,
                from,
                to,
                pageNumber,
                pageSize
            );

            if (response.IsSuccess)
                return Ok(response);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("{interestId}/rent-status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateRentalStatus(int interestId, [FromBody] string status)
        {
            var response = await _interestService.UpdateRentalStatusAsync(interestId, status);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("{id}/paymentAmount")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdatePaymentAmount(int id, [FromBody] PaymentAmountDTO dto)
        {
            var response = await _interestService.UpdatePaymentAmountAsync(id, dto.PaymentAmount);
            if (response.IsSuccess)
                return Ok(response);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("{interestId}/notes")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateNotes(int interestId, [FromBody] string notes)
        {
            var response = await _interestService.UpdateNotesAsync(interestId, notes);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("{interestId}/owner-comment")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateOwnerComment(int interestId, [FromBody] string comment)
        {
            var response = await _interestService.UpdateOwnerCommentAsync(interestId, comment);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("{interestId}/user-comment")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateUserComment(int interestId, [FromBody] string comment)
        {
            var response = await _interestService.UpdateUserCommentAsync(interestId, comment);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("{interestId}/message-owner")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> MessageOwner(int interestId)
        {
            var response = await _interestService.SendMessageToOwnerAsync(interestId);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("{interestId}/message-user")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> MessageUser(int interestId)
        {
            var response = await _interestService.SendMessageToUserAsync(interestId);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("{interestId}/message-owner-payment")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> MessageOwnerRental(int interestId)
        {
            var response = await _interestService.SendPaymentMessageToOwnerAsync(interestId);

            if (response.IsSuccess)
                return Ok(response);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("export")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Export(
        [FromQuery] string? status,
        [FromQuery] string? ownerName,
        [FromQuery] string? userName,
        [FromQuery] string? dressName,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
        {
            var exportList = await _interestService.GetExportListAsync(
                status,
                ownerName,
                userName,
                dressName,
                from,
                to
            );

            var fileBytes = _interestExportService.GenerateExcel(exportList);

            return File(
                //הדפדפן מקבל והופך את זה לקובץ אמיתי
                fileBytes,
                //תתיחס אליו כקובץ אקסל
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Interests.xlsx"
            );
        }

   /*     [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<List<InterestListDTO>>>> GetUserInterests(int userId)
        {
            var response = await _interestService.GetUserInterestsAsync(userId);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }*/
    }
}
