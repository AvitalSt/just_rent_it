using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JustRentItAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<UserResponse>>> Register([FromBody] RegisterDTO user)
        {
            var response = await _authService.RegisterAsync(user);

            if(response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<UserResponse>>> Login([FromBody] LoginDTO user)
        {
            var response = await _authService.LoginAsync(user);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("google-login")]
        public async Task<ActionResult<Response<UserResponse>>> GoogleLogin([FromBody] GoogleLoginDTO googleUser)
        {
            var response = await _authService.GoogleLoginAsync(googleUser);

            if (response.IsSuccess)
                return Ok(response);
            else
                return StatusCode((int)response.StatusCode, response);
        }
    }
}
