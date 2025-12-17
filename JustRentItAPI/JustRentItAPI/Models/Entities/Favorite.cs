using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustRentItAPI.Models.Entities
{
    public class Favorite
    {
        [Key]
        public int FavoriteID { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey(nameof(Dress))]
        public int DressID { get; set; }
        public Dress Dress { get; set; }

        public DateTime SavedDate { get; set; } = DateTime.UtcNow;
    }
}
