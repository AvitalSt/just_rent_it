using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IDressService
    {
        Task<Response<PagedResultDTO<DressListDTO>>> GetFilteredAsync(
               string? city = null,
    string? eventType = null,
    string? saleType = null,
    string? ageGroup = null,
    string? colorGroup = null,
    string? sizeGroup = null,
    string? priceGroup = null,
    string? stateGroup = null,
    string? statusGroup = null,
    string? orderBy = null,
    int pageNumber = 1,
    int pageSize = 24);

        Task<Response<DressDTO>> GetDressByIdAsync(int id);

        Task<Response<DressDTO>> AddDressAsync(AddDressDTO dto);

        Task<Response<DressDTO>> UpdateDressAsync(int id, UpdateDressDTO dto);

        Task<Response<DressDTO>> DeleteDressAsync(int id);

        Task<Response<DressDTO>> ActivateDressAsync(int id);

        Task<Response<UserDTO>> GetDressOwnerAsync(int dressId);

        Task<Response<List<DressListDTO>>> GetMostViewedAsync();

/*        Task<Response<List<DressListDTO>>> GetUserDressesAsync();
*/    }
}
