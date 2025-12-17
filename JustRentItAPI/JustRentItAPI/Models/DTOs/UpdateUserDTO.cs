using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.DTOs
{
    public class UpdateUserDTO
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [EmailAddress, MaxLength(150)]
        public string? Email { get; set; }

        [Phone, MaxLength(20)]
        public string? Phone { get; set; }
    }
}
