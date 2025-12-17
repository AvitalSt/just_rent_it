using JustRentItAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class Interest
    {
        [Key]
        public int InterestID { get; set; }

        public int DressID { get; set; }
        public Dress Dress { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public DateTime SentDate { get; set; }

        public RentStatus Status { get; set; } = RentStatus.Pending;

        public decimal PaymentAmount { get; set; } = 0;

        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string OwnerComment { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string UserComment { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;
        public int OwnerMailCount { get; set; } = 0;
        public int UserMailCount { get; set; } = 0;
    }
}
