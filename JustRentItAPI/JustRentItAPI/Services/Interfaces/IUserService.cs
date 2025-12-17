using JustRentItAPI.Models.DTOs;
using System.Security.Claims;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<UserDTO>> GetCurrentUserAsync(ClaimsPrincipal userClaims);
        Task<Response<UserDTO>> GetUserByIdAsync(int userId);
        Task<Response<UserDTO>> UpdateUserAsync(int userId, UpdateUserDTO dto);
        Task<Response> SendPasswordResetLinkAsync(string email);
        Task<Response<UserResponse>> ResetPasswordAsync(string token, string newPassword);
/*        Task<Response> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
*/
    }
}
