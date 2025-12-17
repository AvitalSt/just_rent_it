using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class Color
    {
        [Key]
        public int ColorID { get; set; }

        [Required, MaxLength(50)]
        public string NameEnglish { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string NameHebrew { get; set; } = string.Empty;

        public ICollection<DressColor> DressColors { get; set; } = new List<DressColor>();
    }

}
