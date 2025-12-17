using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class AgeGroup
    {
        [Key]
        public int AgeGroupID { get; set; }

        [Required, MaxLength(50)]
        public string NameEnglish { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string NameHebrew { get; set; } = string.Empty;

        public ICollection<DressAgeGroup> DressAgeGroups { get; set; } = new List<DressAgeGroup>();
    }
}
