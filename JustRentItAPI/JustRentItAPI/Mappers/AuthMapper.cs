using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;

namespace JustRentItAPI.Mappers
{
    public class AuthMapper
    {
        public static UserDTO ToUserDTO(User user, List<int> wishlistIds)
        {
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
}
