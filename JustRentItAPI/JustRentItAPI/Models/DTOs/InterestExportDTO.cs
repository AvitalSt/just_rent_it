using JustRentItAPI.Models.Enums;

namespace JustRentItAPI.Models.DTOs
{
    public class InterestExportDTO
    {
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string DressName { get; set; } = string.Empty;
        public RentStatus Status { get; set; }
        public decimal Price { get; set; }  
        public string Notes { get; set; } = string.Empty;
        public string OwnerComment { get; set; } = string.Empty;
        public string OwnerEmail { get; set; } = string.Empty;
        public string UserComment { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
