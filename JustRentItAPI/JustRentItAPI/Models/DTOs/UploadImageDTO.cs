using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.DTOs
{
    public class UploadImageDTO
    {
        [Required]
        //default! זה אומר אני יודעת השקובל לא יהיה"נאל" אז אני נותן לו ערך ברירת חדל כדי שהקומפיילר לא יתלונן
        public IFormFile File { get; set; } = default!;

        public int? DressID { get; set; } // אם זה תמונה לשמלה קיימת
    }
}
