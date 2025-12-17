using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Models.Enums;

namespace JustRentItAPI.Repositories.Interfaces
{
    public interface IDressRepository
    {
        Task<List<Dress>> GetAllForCatalogAsync();

        Task<PagedResultDTO<Dress>> GetFilteredAsync(
            List<int>? cityIDs = null,
            List<int>? eventTypeIDs = null,
            SaleType? saleType = null,
            List<int>? ageGroupIDs = null,
            List<int>? colorIDs = null,
            List<int>? sizeIDs = null,
            string? priceGroup = null,
            List<DressState>? stateGroup = null,
            List<DressStatus>? statusGroup = null,
            string? orderBy = null,
            int pageNumber = 1,
            int pageSize = 24,
            bool isAdmin = false);

        Task<Dress?> GetByIdAsync(int id);

        Task DeleteImageAsync(DressImage image);

        Task<Dress> AddAsync(Dress dress);

        Task UpdateAsync(Dress dress);

        Task<List<Dress>> GetByIdsAsync(List<int> ids);

        Task<User?> GetOwnerByIdAsync(int userId);

        Task<List<Dress>> GetMostViewedAsync(int count = 8);

/*        Task<List<Dress>> GetByUserIdAsync(int userId);
*/    }
}
