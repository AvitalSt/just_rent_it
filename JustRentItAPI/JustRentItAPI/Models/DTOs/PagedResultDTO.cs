namespace JustRentItAPI.Models.DTOs
{
    public class PagedResultDTO<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int? maxPrice { get; set; }
    }
}
