using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using JustRentItAPI.Utils;
using JustRentItAPI.Validation;
using System.Net;
using System.Security.Claims;

namespace JustRentItAPI.Services.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly IMailService _mailService;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, TokenService tokenService, IMailService mailService, IWishlistRepository wishlistRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mailService = mailService;
            _wishlistRepository = wishlistRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<Response<UserDTO>> GetCurrentUserAsync(ClaimsPrincipal userClaims)
        {
            if (userClaims == null || userClaims.Identity?.IsAuthenticated != true)
            {
                return new Response<UserDTO>
                {
                    IsSuccess = false,
                    Message = "User is not authenticated.",
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }

            var userIdClaim = userClaims.FindFirst("UserID");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return new Response<UserDTO>
                {
                    IsSuccess = false,
                    Message = "User ID not found or invalid in token.",
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }

            return await GetUserByIdAsync(userId);
        }

        public async Task<Response<UserDTO>> GetUserByIdAsync(int userId)
        {
            try
            {
                int? userIdFromContext = UserHelper.GetUserId(_httpContextAccessor);
                bool userIsAdmin = UserHelper.IsAdmin(_httpContextAccessor);
                if (userIdFromContext != userId && !userIsAdmin)
                    return new Response<UserDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.Forbidden,
                        Message = "You do not have access to this user"
                    };

                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)                
                    return new Response<UserDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "משתמש לא נמצא במערכת"
                    };

                var dto = await UserMapper.ToDTO(user, _wishlistRepository);

                return new Response<UserDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "User details retrieved successfully.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new Response<UserDTO>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<UserDTO>> UpdateUserAsync(int userId, UpdateUserDTO dto)
        {
            try
            {
                int? userIdFromContext = UserHelper.GetUserId(_httpContextAccessor);
                bool userIsAdmin = UserHelper.IsAdmin(_httpContextAccessor);
                if (userIdFromContext != userId && userIsAdmin)
                    return new Response<UserDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.Forbidden,
                        Message = "You do not have access to this user"
                    };

                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new Response<UserDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "משתמש לא נמצא במערכת"
                    };
                }

                if (dto.FirstName != null)
                {
                    if (!dto.FirstName.IsValidName())
                        return new Response<UserDTO>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "First name is invalid."
                        };
                    user.FirstName = dto.FirstName;
                }

                if (dto.LastName != null)
                {
                    if (!dto.LastName.IsValidName())
                        return new Response<UserDTO>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "Last name is invalid."
                        };
                    user.LastName = dto.LastName;
                }

                if (dto.Email != null)
                {
                    if (!dto.Email.IsValidEmail())
                        return new Response<UserDTO>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "Email is invalid."
                        };
                    user.Email = dto.Email;
                }

                if (dto.Phone != null)
                {
                    if (!dto.Phone.IsValidPhone())
                        return new Response<UserDTO>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "Phone number is invalid."
                        };
                    user.Phone = dto.Phone;
                }

                user.UpdateAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                var resultDto = await UserMapper.ToDTO(user, _wishlistRepository);

                return new Response<UserDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "User details updated successfully.",
                    Data = resultDto
                };
            }
            catch (Exception ex)
            {
                return new Response<UserDTO>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> SendPasswordResetLinkAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                    return new Response<UserDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "משתמש לא נמצא במערכת"
                    };

                var token = Guid.NewGuid().ToString();

                user.PasswordResetToken = token;
                var minutes = int.Parse(_configuration["PasswordResetTokenMinutes"]);
                user.TokenExpiration = DateTime.UtcNow.AddMinutes(minutes);

                await _userRepository.UpdateAsync(user);

                var clientBaseUrl = _configuration["ClientBaseUrl"];
                string resetLink = $"{clientBaseUrl}/reset-password/{token}";
                var emailResponse = await _mailService.SendPasswordResetEmailAsync(user, resetLink);

                if (!emailResponse.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Failed to send password reset email.",
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Password reset email sent successfully.",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<UserResponse>> ResetPasswordAsync(string token, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetByResetTokenAsync(token);

                if (user == null || !user.TokenExpiration.HasValue || user.TokenExpiration < DateTime.UtcNow)
                {
                    return new Response<UserResponse>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Invalid or expired token"
                    };
                }

                if (!newPassword.IsValidPassword())
                    return new Response<UserResponse>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "סיסמה חייבת להכיל 8 תווים, אותיות גדולות וקטנות, ספרה ותו מיוחד."
                    };

                var Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

                user.Password = Password;
                user.PasswordResetToken = null;
                user.TokenExpiration = null;

                await _userRepository.UpdateAsync(user);

                var newToken = _tokenService.GenerateToken(user.UserID, user.Email, user.Role, TimeSpan.FromHours(1));

                var UserDTO = await UserMapper.ToDTO(user, _wishlistRepository);

                return new Response<UserResponse>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Password reset successful. You are now logged in.",
                    Data = new UserResponse
                    {
                        User = UserDTO,
                        Token = newToken
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        /*        public async Task<Response> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                int? userIdFromContext = UserHelper.GetUserId(_httpContextAccessor);
                bool userIsAdmin = UserHelper.IsAdmin(_httpContextAccessor);
                if (userIdFromContext != userId && userIsAdmin)
                    return new Response<UserDTO>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.Forbidden,
                        Message = "You do not have access to this user"
                    };

                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "User not found"
                    };
                }

                if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Old password is incorrect"
                    };

                if (!PasswordValidation.IsValidPassword(newPassword))
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "New password does not meet security requirements"
                    };

                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.UpdateAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Password changed successfully"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }
*/
    }
}
