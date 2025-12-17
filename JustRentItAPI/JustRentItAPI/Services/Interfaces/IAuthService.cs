using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Response<UserResponse>> RegisterAsync(RegisterDTO newUser);
        Task<Response<UserResponse>> LoginAsync(LoginDTO user);
        Task<Response<UserResponse>> GoogleLoginAsync(GoogleLoginDTO googleUser);
    }
}
