using System.ComponentModel.DataAnnotations;

namespace JustRentItAPI.Models.DTOs
{
    public class AddDressDTO
    {
        //Required אם לא ממלאים שדה הוא - אז חוזר הודעת שיגאה 400 עם ההודעה שהגדרנו
        //[Range(1, int.MaxValue)] ENUM מוודא שהמספר הוא לפחות 1

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        public List<int> ColorIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Size is required.")]
        public List<int> SizeIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "City is required.")]
        public List<int> CityIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Sale type is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Sale type is required.")]
        public int SaleType { get; set; }

        [Required(ErrorMessage = "Age group is required.")]
        public List<int> AgeGroupIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Event type is required.")]
        public List<int> EventTypeIds { get; set; } = new List<int>();

        public List<string> ImagePaths { get; set; } = new List<string>();

        [Required(ErrorMessage = "Main image is required.")]
        [MaxLength(300, ErrorMessage = "Main image path cannot exceed 300 characters.")]
        public string MainImage { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "State is required.")]
        public int State { get; set; }
    }
}
