using JustRentItAPI.Data;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JustRentItAPI.Repositories.Classes
{
    public class AuthRepository:IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return await _context.Users.FirstAsync(u => u.Email == newUser.Email);
        }

        public async Task<User?> IsExistingUserAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
