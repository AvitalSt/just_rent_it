using JustRentItAPI.Data;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JustRentItAPI.Repositories.Classes
{
    public class WishlistRepository: IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddFavoriteAsync(int userId, int dressId)
        {
            var existing = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserID == userId && f.DressID == dressId);

            if (existing == null)
            {
                _context.Favorites.Add(new Favorite
                {
                    UserID = userId,
                    DressID = dressId,
                    SavedDate = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFavoriteAsync(int userId, int dressId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserID == userId && f.DressID == dressId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<int>> GetUserFavoriteDressIdsAsync(int userId)
        {
            return await _context.Favorites
                .Where(f => f.UserID == userId)
                .Select(f => f.DressID)
                .ToListAsync();
        }
    }
}
