using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.Entities
{
    public class EventType
    {
        [Key]
        public int EventTypeID { get; set; }

        [Required, MaxLength(50)]
        public string NameEnglish { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string NameHebrew { get; set; } = string.Empty;

        public ICollection<DressEventType> DressEventTypes { get; set; } = new List<DressEventType>();
    }
}
