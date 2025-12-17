using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class DressEventType
    {
        [Required]
        public int DressID { get; set; }
        public Dress Dress { get; set; }

        [Required]
        public int EventTypeID { get; set; }
        public EventType EventType { get; set; }
    }
}
