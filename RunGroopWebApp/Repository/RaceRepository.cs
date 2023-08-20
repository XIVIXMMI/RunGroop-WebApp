using System;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
	public class RaceRepository : IRaceRepository
	{
        private readonly ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context)
		{
            _context = context;
        }

        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Races
                .Include(i => i.Address)
                .FirstOrDefaultAsync(i => i.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<IEnumerable<Race>> GetRacesByCity(string city)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return await _context.Races
                .Where(c => c.Address.City
                .Contains(city))
                .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}

