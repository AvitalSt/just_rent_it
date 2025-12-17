using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class DressAgeGroup
    {
        [Required]
        public int DressID { get; set; }
        public Dress Dress { get; set; }

        [Required]
        public int AgeGroupID { get; set; }
        public AgeGroup AgeGroup { get; set; }
    }
}
