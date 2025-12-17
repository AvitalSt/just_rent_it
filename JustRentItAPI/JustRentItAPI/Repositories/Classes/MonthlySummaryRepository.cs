using JustRentItAPI.Data;
using JustRentItAPI.Models.Entities;
using JustRentItAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JustRentItAPI.Repositories.Classes
{
    public class MonthlySummaryRepository : IMonthlySummaryRepository
    {
        private readonly AppDbContext _context;

        public MonthlySummaryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MonthlySummary summary)
        {
            _context.MonthlySummaries.Add(summary);
            await _context.SaveChangesAsync();
        }

        public async Task<MonthlySummary?> GetLastAsync()
        {
            return await _context.MonthlySummaries
                .OrderByDescending(m => m.SentAt)
                .FirstOrDefaultAsync();
        }
    }
}
