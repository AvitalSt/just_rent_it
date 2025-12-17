using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;

namespace JustRentItAPI.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(User newUser);
        Task<User?> IsExistingUserAsync(string email);
    }
}
