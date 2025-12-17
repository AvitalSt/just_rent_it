using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustRentItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<ActionResult<Response<UserDTO>>> GetCurrentUser()
        {
            var response = await _userService.GetCurrentUserAsync(User);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("{userId}")]
        [Authorize]
        public async Task<ActionResult<Response<UserDTO>>> UpdateUser(int userId, [FromBody] UpdateUserDTO dto)
        {
            var response = await _userService.UpdateUserAsync(userId, dto);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult<Response>> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var response = await _userService.SendPasswordResetLinkAsync(dto.Email);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<Response<UserResponse>>> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var response = await _userService.ResetPasswordAsync(dto.Token, dto.NewPassword);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

       /* [HttpPut("{userId}/change-password")]
        [Authorize]
        public async Task<ActionResult<Response>> ChangePassword(int userId, [FromBody] ChangePasswordDTO dto)
        {
            var response = await _userService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }*/
    }
}