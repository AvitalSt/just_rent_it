using JustRentItAPI.Models.Entities;

namespace JustRentItAPI.Models.DTOs
{
    public class UserResponse
    {
        public UserDTO? User { get; set; }
        public string? Token { get; set; } = string.Empty;
    }
}
