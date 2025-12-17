namespace JustRentItAPI.Models.DTOs
{
    public class DressOptionsDTO
    {
        public List<SizeDTO> Sizes { get; set; } = new();
        public List<ColorDTO> Colors { get; set; } = new();
        public List<CityDTO> Cities { get; set; } = new();
        public List<AgeGroupDTO> AgeGroups { get; set; } = new();
        public List<EventTypeDTO> EventTypes { get; set; } = new();
    }
}
