using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Repositories.Interfaces
{
    public interface IWishlistRepository
    {
        Task AddFavoriteAsync(int userId, int dressId);
        Task RemoveFavoriteAsync(int userId, int dressId);
        Task<List<int>> GetUserFavoriteDressIdsAsync(int userId);
    }
}
