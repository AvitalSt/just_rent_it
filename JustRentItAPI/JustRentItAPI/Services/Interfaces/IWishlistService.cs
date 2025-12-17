using JustRentItAPI.Models.DTOs;

namespace JustRentItAPI.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<Response> AddToWishlistAsync(int dressId);
        Task<Response> RemoveFromWishlistAsync(int dressId);
        Task<Response<List<DressListDTO>>> GetUserWishlistAsync();
        Task<Response<List<DressListDTO>>> GetDressesByIdsAsync(string ids);
    }
}
