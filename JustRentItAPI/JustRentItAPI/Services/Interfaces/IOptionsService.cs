using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IOptionsService
    {
        Task<Response<List<SizeDTO>>> GetSizesAsync();
        Task<Response<List<AgeGroupDTO>>> GetAgeGroupsAsync();
        Task<Response<List<CityDTO>>> GetCitiesAsync();
        Task<Response<List<EventTypeDTO>>> GetEventTypesAsync();
        Task<Response<List<ColorDTO>>> GetColorsAsync();
    }
}
