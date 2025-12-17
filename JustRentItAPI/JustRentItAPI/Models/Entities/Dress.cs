using JustRentItAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustRentItAPI.Models.Entities
{
    public class Dress
    {
        [Key]
        public int DressID { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User User { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DressState State { get; set; }

        [Required]
        public DressStatus Status { get; set; }

        [Required]
        public SaleType SaleType { get; set; }

        [Required]
        public int Price { get; set; }

        [MaxLength(300)]
        public string MainImage { get; set; } = string.Empty;

        public int Views { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<DressColor> Colors { get; set; } = new List<DressColor>();
        public ICollection<DressSize> Sizes { get; set; } = new List<DressSize>();
        public ICollection<DressAgeGroup> AgeGroups { get; set; } = new List<DressAgeGroup>();
        public ICollection<DressEventType> EventTypes { get; set; } = new List<DressEventType>();
        public ICollection<DressCity> Cities { get; set; } = new List<DressCity>();

        public ICollection<DressImage> Images { get; set; } = new List<DressImage>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Interest> Interests { get; set; } = new List<Interest>();
    }
}
