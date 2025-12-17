using JustRentItAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Password { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        public string? PasswordResetToken { get; set; }

        public DateTime? TokenExpiration { get; set; }

        public DateTime UpdateAt  { get; set; } 

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Dress> Dresses { get; set; } = new List<Dress>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Interest> Interests { get; set; } = new List<Interest>();
    }
}
