using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class City
    {
        [Key]
        public int CityID { get; set; }

        [Required, MaxLength(50)]
        public string NameEnglish { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string NameHebrew { get; set; } = string.Empty;

        public ICollection<DressCity> DressCities { get; set; } = new List<DressCity>();
    }
}
