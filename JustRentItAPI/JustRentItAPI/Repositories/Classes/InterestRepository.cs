using JustRentItAPI.Data;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Models.Enums;
using JustRentItAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JustRentItAPI.Repositories.Classes
{
    public class InterestRepository : IInterestRepository
    {
        private readonly AppDbContext _context;

        public InterestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountUserInterestsToday(int userId)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            return await _context.Interests
             .Where(i => i.UserID == userId && i.SentDate >= today && i.SentDate < tomorrow)
             .CountAsync();
        }

        public async Task AddAsync(Interest interest)
        {
            await _context.Interests.AddAsync(interest);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResultDTO<Interest>> GetFilteredAsync(
        string? status = null,
        string? ownerName = null,
        string? userName = null,
        string? dressName = null,
        DateTime? from = null,
        DateTime? to = null,
        int pageNumber = 1,
        int pageSize = 50)
        {
            var query = _context.Interests
                .Include(i => i.Dress)
                    .ThenInclude(d => d.User)
                .Include(i => i.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                var statuses = status
                    .Split('_')
                    .Select(int.Parse)
                    .ToList();

                query = query.Where(i => statuses.Contains((int)i.Status));
            }

            if (!string.IsNullOrWhiteSpace(ownerName))
            {
                query = query.Where(i =>
                    (i.Dress.User.FirstName + " " + i.Dress.User.LastName)
                        .Contains(ownerName));
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(i =>
                    (i.User.FirstName + " " + i.User.LastName)
                        .Contains(userName));
            }

            if (!string.IsNullOrWhiteSpace(dressName))
            {
                query = query.Where(i => i.Dress.Name.Contains(dressName));
            }

            if (from.HasValue)
            {
                query = query.Where(i => i.SentDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(i => i.SentDate <= to.Value);
            }

            query = query.OrderByDescending(i => i.SentDate);

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDTO<Interest>
            {
                Items = items,
                TotalCount = totalCount,
                maxPrice = 0 
            };
        }

        public async Task<Interest?> GetByIdAsync(int interestId)
        {
            return await _context.Interests
                .Include(i => i.Dress)
                    .ThenInclude(d => d.User) 
                .Include(i => i.User)      
                .FirstOrDefaultAsync(i => i.InterestID == interestId);
        }

        public async Task UpdateAsync(Interest interest)
        {
            _context.Interests.Update(interest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Interest>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.Interests
                .Where(i => i.SentDate >= from && i.SentDate <= to)
                .Include(i => i.User)
                .Include(i => i.Dress)
                    .ThenInclude(d => d.User)
                .ToListAsync();
        }

/*        public async Task<List<Interest>> GetUserInterestsAsync(int userId)
        {
            return await _context.Interests
                .Include(i => i.User)          
                .Include(i => i.Dress)         
                    .ThenInclude(d => d.User) 
                .Where(i => i.UserID == userId)
                .ToListAsync();
        }*/
    }
}
