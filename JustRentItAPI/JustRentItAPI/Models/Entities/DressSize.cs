using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class DressSize
    {
        [Required]
        public int DressID { get; set; }
        public Dress Dress { get; set; }

        [Required]
        public int SizeID { get; set; }
        public Size Size { get; set; }
    }
}
