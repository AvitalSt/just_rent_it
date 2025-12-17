namespace JustRentItAPI.Models.DTOs
{
    public class DressImageDTO
    {
        public int ImageID { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}
