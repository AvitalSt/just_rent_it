using JustRentItAPI.Repositories.Interfaces;

namespace JustRentItAPI.Services
{
    public static class UserHelper
    {
        public static int? GetUserId(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user != null && user.Identity?.IsAuthenticated == true)
                return int.Parse(user.FindFirst("UserID")!.Value);
            return null;
        }

        public static bool IsAdmin(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user != null && user.Identity?.IsAuthenticated == true && user.IsInRole("Admin");
        }

        public static async Task<List<int>> GetUserFavoriteDressIdsAsync(
            IHttpContextAccessor httpContextAccessor,
            IWishlistRepository favoriteRepository)
        {
            var userFavoriteDressIds = new List<int>();
            var user = httpContextAccessor.HttpContext?.User;

            if (user != null && user.Identity?.IsAuthenticated == true)
            {
                int userId = int.Parse(user.FindFirst("UserID")!.Value);
                userFavoriteDressIds = await favoriteRepository.GetUserFavoriteDressIdsAsync(userId);
            }

            return userFavoriteDressIds;
        }
    }
}
