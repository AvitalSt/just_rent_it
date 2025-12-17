using JustRentItAPI.Models.Enums;

namespace JustRentItAPI.Models.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<int> WishlistDressIds { get; set; } = new();
    }
}
