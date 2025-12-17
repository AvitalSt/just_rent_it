
using JustRentItAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.DTOs
{
    public class UpdateDressDTO
    {
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public int? Price { get; set; }

        public List<int>? ColorIDs { get; set; }

        public List<int>? SizeIDs { get; set; }

        public List<int>? CityIDs { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Sale type is invalid.")]
        public SaleType? SaleType { get; set; }

        public List<int>? AgeGroupIDs { get; set; }

        public List<int>? EventTypeIDs { get; set; }

        public DressState? State { get; set; }

        public DressStatus? Status { get; set; } // רק Admin יכול לשנות

        [MaxLength(300, ErrorMessage = "Main image path cannot exceed 300 characters.")]
        public string? MainImage { get; set; }

        public List<string> AddImagePaths { get; set; } = new();
        public List<int> RemoveImageIds { get; set; } = new();
    }
}
