using JustRentItAPI.Data;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JustRentItAPI.Repositories.Classes
{
    public class OptionsRepository: IOptionsRepository
    {
        private readonly AppDbContext _context;

        public OptionsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SizeDTO>> GetAllSizesAsync()
        {
            return await _context.Sizes
                .Select(s => new SizeDTO
                {
                    Id = s.SizeID,
                    Name = s.Name
                })
                .ToListAsync();
        }

        public async Task<List<AgeGroupDTO>> GetAllAgeGroupsAsync()
        {
            return await _context.AgeGroups
                 .OrderBy(s => s.NameHebrew)
                .Select(a => new AgeGroupDTO
                {
                    Id = a.AgeGroupID,
                    NameHebrew = a.NameHebrew
                })
                .ToListAsync();
        }

        public async Task<List<CityDTO>> GetAllCitiesAsync()
        {
            return await _context.Cities
                 .OrderBy(s => s.NameHebrew)
                .Select(c => new CityDTO
                {
                    Id = c.CityID,
                    NameHebrew = c.NameHebrew
                })
                .ToListAsync();
        }

        public async Task<List<EventTypeDTO>> GetAllEventTypesAsync()
        {
            return await _context.EventTypes
                 .OrderBy(s => s.NameHebrew)
                .Select(e => new EventTypeDTO
                {
                    Id = e.EventTypeID,
                    NameHebrew = e.NameHebrew
                })
                .ToListAsync();
        }

        public async Task<List<ColorDTO>> GetAllColorsAsync()
        {
            return await _context.Colors
                .OrderBy(c => c.NameHebrew)
                .Select(c => new ColorDTO
                {
                    Id = c.ColorID,
                    NameHebrew = c.NameHebrew
                })
                .ToListAsync();
        }

    }
}
