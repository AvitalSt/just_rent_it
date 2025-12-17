using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;

public static class UserMapper
{
    public static async Task<UserDTO> ToDTO(User user, IWishlistRepository wishlistRepository)
    {
        var wishlistIds = await wishlistRepository.GetUserFavoriteDressIdsAsync(user.UserID);

        return new UserDTO
        {
            UserID = user.UserID,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            CreatedDate = user.CreatedDate,
            UpdateAt = user.UpdateAt,
            WishlistDressIds = wishlistIds
        };
    }
}
