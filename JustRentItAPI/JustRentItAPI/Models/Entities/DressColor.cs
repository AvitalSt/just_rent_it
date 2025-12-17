using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class DressColor
    {

        [Required]
        public int DressID { get; set; }
        public Dress Dress { get; set; }

        [Required]
        public int ColorID { get; set; }
        public Color Color { get; set; }
    }
}
