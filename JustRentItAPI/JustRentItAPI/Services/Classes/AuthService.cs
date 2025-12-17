using Google.Apis.Auth;
using JustRentItAPI.Mappers;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Models.Enums;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Interfaces;
using JustRentItAPI.Utils;
using JustRentItAPI.Validation;
using System.Net;

namespace JustRentItAPI.Services.Classes
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly TokenService _tokenService;
        private readonly IWishlistRepository _wishlistRepository;

        public AuthService(IAuthRepository authRepository, TokenService tokenService, IWishlistRepository wishlistRepository)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _wishlistRepository = wishlistRepository;
        }

        public async Task<Response<UserResponse>> RegisterAsync(RegisterDTO newUser)
        {
            var (firstName, lastName, password, email, phone) = (newUser.FirstName, newUser.LastName, newUser.Password, newUser.Email, newUser.Phone);

            if (!firstName.IsValidName())
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "שם פרטי אינו תקין.",
                };

            if (!lastName.IsValidName())
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "שם משפחה אינו תקין.",
                };

            if (!password.IsValidPassword())
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "סיסמה חייבת להכיל 8 תווים, אותיות גדולות וקטנות, ספרה ותו מיוחד."
                };

            if (!email.IsValidEmail())
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "כתובת המייל אינה תקינה.",
                };

            if (!phone.IsValidPhone())
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "מספר הטלפון אינו תקין.",
                };

            var result = await _authRepository.IsExistingUserAsync(email);

            if (result != null)
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Conflict,
                    Message = "המשתמש כבר קיים.",
                };

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Phone = phone,
                Role = UserRole.User,
                CreatedDate = DateTime.UtcNow.Date
            };

            var response = await _authRepository.RegisterAsync(user);

            var tokenExpiry = newUser.RememberMe ? TimeSpan.FromDays(30) : TimeSpan.FromHours(1);
            var token = _tokenService.GenerateToken(response.UserID, email, user.Role, tokenExpiry);

            var wishlistIds = await _wishlistRepository.GetUserFavoriteDressIdsAsync(response.UserID);
            var UserDTO = AuthMapper.ToUserDTO(response, wishlistIds);
            return new Response<UserResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "המשתמש נרשם בהצלחה.",
                Data = new UserResponse
                {
                    User = UserDTO,
                    Token = token
                }
            };
        }

        public async Task<Response<UserResponse>> LoginAsync(LoginDTO user)
        {
            var (email, password) = (user.Email, user.Password);
            var result = await _authRepository.IsExistingUserAsync(email);

            if (result != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(password, result.Password))
                    return new Response<UserResponse>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "סיסמה שגויה.",
                    };

                var tokenExpiry = user.RememberMe ? TimeSpan.FromDays(30) : TimeSpan.FromHours(1);
                var token = _tokenService.GenerateToken(result.UserID, user.Email, result.Role, tokenExpiry);

                var wishlistIds = await _wishlistRepository.GetUserFavoriteDressIdsAsync(result.UserID);
                var UserDTO = AuthMapper.ToUserDTO(result, wishlistIds);
                return new Response<UserResponse>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "התחברות בוצעה בהצלחה.",
                    Data = new UserResponse
                    {
                        User = UserDTO,
                        Token = token,
                    }
                };
            }
            else
            {
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "המשתמש לא נמצא.",
                };
            }
        }

        public async Task<Response<UserResponse>> GoogleLoginAsync(GoogleLoginDTO googleUser)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleUser.IdToken);

                var userEntity = await _authRepository.IsExistingUserAsync(payload.Email);
                if (userEntity == null)
                {
                    return new Response<UserResponse>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "המשתמש לא קיים במערכת. יש לבצע קודם הירשמות."
                    };
                }

                var tokenExpiry = googleUser.RememberMe ? TimeSpan.FromDays(30) : TimeSpan.FromHours(1);
                var token = _tokenService.GenerateToken(userEntity.UserID, userEntity.Email, userEntity.Role, tokenExpiry);

                var wishlistIds = await _wishlistRepository.GetUserFavoriteDressIdsAsync(userEntity.UserID);
                var UserDTO = AuthMapper.ToUserDTO(userEntity, wishlistIds);

                return new Response<UserResponse>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "התחברות עם Google בוצעה בהצלחה.",
                    Data = new UserResponse
                    {
                        User = UserDTO,
                        Token = token
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<UserResponse>
                {
                    IsSuccess = false,
                    Message = $"אירעה שגיאה: {ex.Message}",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}
