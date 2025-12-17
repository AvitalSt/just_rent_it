using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class DressCity
    {
        [Required]
        public int DressID { get; set; }
        public Dress Dress { get; set; }

        [Required]
        public int CityID { get; set; }
        public City City { get; set; }
    }
}
