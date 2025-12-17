using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustRentItAPI.Models.Entities
{
    public class DressImage
    {
        [Key]
        public int DressImageID { get; set; }

        [Required]
        [ForeignKey(nameof(Dress))]
        public int DressID { get; set; }

        public Dress Dress { get; set; } = null!;

        [Required, MaxLength(300)]
        public string ImagePath { get; set; } = string.Empty;

        public bool IsMain { get; set; } = false;
    }
}
