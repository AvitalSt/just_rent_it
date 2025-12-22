using JustRentItAPI.Data;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Models.Enums;
using JustRentItAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace JustRentItAPI.Repositories.Classes
{
    public class DressRepository : IDressRepository
    {
        private readonly AppDbContext _context;

        public DressRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        private IQueryable<Dress> GetReadQuery()
        {
            return _context.Dresses
                .AsNoTracking()
                .Include(d => d.User)
                .Include(d => d.Colors).ThenInclude(dc => dc.Color)
                .Include(d => d.Sizes).ThenInclude(ds => ds.Size)
                .Include(d => d.Cities).ThenInclude(dc => dc.City)
                .Include(d => d.AgeGroups).ThenInclude(da => da.AgeGroup)
                .Include(d => d.EventTypes).ThenInclude(de => de.EventType)
                .Include(d => d.Images);
        }

        private IQueryable<Dress> GetTrackingQuery()
        {
            return _context.Dresses
                .Include(d => d.User)
                .Include(d => d.Colors).ThenInclude(dc => dc.Color)
                .Include(d => d.Sizes).ThenInclude(ds => ds.Size)
                .Include(d => d.Cities).ThenInclude(dc => dc.City)
                .Include(d => d.AgeGroups).ThenInclude(da => da.AgeGroup)
                .Include(d => d.EventTypes).ThenInclude(de => de.EventType)
                .Include(d => d.Images);
        }

        public async Task<List<Dress>> GetAllForCatalogAsync()
        {
            return await _context.Dresses
            .Where(d => d.Status == DressStatus.Active)
            .Include(d => d.Images)
            .ToListAsync();
        }

        public async Task<PagedResultDTO<Dress>> GetFilteredAsync(
            List<int>? cityIDs = null,
            List<int>? eventTypeIDs = null,
            SaleType? saleType = null,
            List<int>? ageGroupIDs = null,
            List<int>? colorIDs = null,
            List<int>? sizeIDs = null,
            List<DressState>? stateGroup = null,
            List<DressStatus>? statusGroup = null,
            string? orderBy = null,
            int pageNumber = 1,
            int pageSize = 24,
            bool isAdmin = false)
        {
            //AsNoTracking זה אובייקטים לקריאה בלבד אל תעקוב אחריהם- גורם לזה להיות מהיר יותר
            //AsQueryable - IQueryable שאילתה שניתן להוסיף עליה עוד תנאים- לפני שהיא פועלת במוסד נתונים- שאילתות דינמיות 
            var query = GetReadQuery().AsQueryable();

            if (colorIDs != null && colorIDs.Any())
            {
                query = query.Where(d => d.Colors.Any(dc => colorIDs.Contains(dc.ColorID)));
            }

            if (sizeIDs != null && sizeIDs.Any())
            {
                query = query.Where(d => d.Sizes.Any(ds => sizeIDs.Contains(ds.SizeID)));
            }

            if (ageGroupIDs != null && ageGroupIDs.Any())
            {
                query = query.Where(d => d.AgeGroups.Any(da => ageGroupIDs.Contains(da.AgeGroupID)));
            }

            if (eventTypeIDs != null && eventTypeIDs.Any())
            {
                query = query.Where(d => d.EventTypes.Any(de => eventTypeIDs.Contains(de.EventTypeID)));
            }

            if (cityIDs != null && cityIDs.Any())
            {
                query = query.Where(d => d.Cities.Any(dc => cityIDs.Contains(dc.CityID)));
            }

            if (saleType.HasValue)
            {
                query = query.Where(d => d.SaleType == saleType.Value);
            }

            if (stateGroup != null && stateGroup.Any())
            {
                query = query.Where(d => stateGroup.Contains(d.State));
            }
            if (!isAdmin)
            {
                query = query.Where(d => d.Status == DressStatus.Active);
            }
            else if (statusGroup != null && statusGroup.Any())
            {
                query = query.Where(d => statusGroup.Contains(d.Status));
            }

            switch (orderBy?.ToLower())
            {
                case "price_asc":
                    query = query.OrderBy(d => d.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(d => d.Price);
                    break;
                case "most_viewed":
                    query = query.OrderByDescending(d => d.Views);
                    break;
                default:
                    query = query.OrderByDescending(d => d.CreatedDate);
                    break;
            }

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var totalCount = await query.CountAsync();

            var dresses = await query
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();

            return new PagedResultDTO<Dress>
            {
                Items = dresses,
                TotalCount = totalCount,
            };
        }

        public async Task<Dress?> GetByIdAsync(int id)
        {
            return await GetReadQuery().FirstOrDefaultAsync(d => d.DressID == id);
        }

        public async Task DeleteImageAsync(DressImage image)
        {
            _context.DressImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<Dress> AddAsync(Dress dress)
        {
            _context.Dresses.Add(dress);
            await _context.SaveChangesAsync();

            //EF לא טוען קשרים אוטומטית בעת שמירה ולכן אני שולפת את האובייקט מחדש עם Include כדי לקבל אובייקט מלא למיפוי ל־DTO ולמניעת Lazy Loading
            var savedDress = await GetReadQuery()
                .FirstOrDefaultAsync(d => d.DressID == dress.DressID);

            return savedDress;
        }

        public async Task UpdateAsync(Dress dress)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Dress>> GetByIdsAsync(List<int> ids)
        {
            return await _context.Dresses
                .Where(d => ids.Contains(d.DressID))
                .Include(d => d.User)
                .Include(d => d.Images)
                .ToListAsync();
        }

        public async Task<User?> GetOwnerByIdAsync(int userId)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserID == userId);
        }

        public async Task<List<Dress>> GetMostViewedAsync(int count = 8)
        {
            return await _context.Dresses
                .Where(d => d.Status == DressStatus.Active)
                .OrderByDescending(d => d.Views)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Dress?> GetByIdForUpdateAsync(int id)
        {
            return await GetTrackingQuery()
                .FirstOrDefaultAsync(d => d.DressID == id);
        }
        /*     public async Task<List<Dress>> GetByUserIdAsync(int userId)
             {
                 return await _context.Dresses.Where(d => d.UserID == userId && d.Status == DressStatus.Active).ToListAsync();
             }*/
    }
}
