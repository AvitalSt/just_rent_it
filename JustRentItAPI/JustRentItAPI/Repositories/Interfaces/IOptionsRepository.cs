using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Repositories.Interfaces
{
    public interface IOptionsRepository
    {
        Task<List<SizeDTO>> GetAllSizesAsync();
        Task<List<AgeGroupDTO>> GetAllAgeGroupsAsync();
        Task<List<CityDTO>> GetAllCitiesAsync();
        Task<List<EventTypeDTO>> GetAllEventTypesAsync();
        Task<List<ColorDTO>> GetAllColorsAsync();
    }
}
