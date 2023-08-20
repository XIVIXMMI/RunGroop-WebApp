using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
	public class ClubRepository : IClubRepository
	{
        private readonly ApplicationDbContext _context;

        public ClubRepository(ApplicationDbContext context)
		{
            _context = context;
        }

        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Clubs
                 .Include(i => i.Address)
                 .FirstOrDefaultAsync(i => i.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<IEnumerable<Club>> GetClubsByCity(string city)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return await _context.Clubs
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

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}

