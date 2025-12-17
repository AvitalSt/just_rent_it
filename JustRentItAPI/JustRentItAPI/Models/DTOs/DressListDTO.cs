namespace JustRentItAPI.Models.DTOs
{
    public class DressListDTO
    {
        public int DressID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string MainImage { get; set; } = string.Empty;
        public bool IsFavorite { get; set; } = false;
    }
}
