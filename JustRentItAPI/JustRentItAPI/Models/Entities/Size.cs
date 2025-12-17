using JustRentItAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class Size
    {
        [Key]
        public int SizeID { get; set; }

        [Required, MaxLength(10)]
        public string Name { get; set; } = string.Empty;

        public ICollection<DressSize> DressSizes { get; set; } = new List<DressSize>();
    }
}
