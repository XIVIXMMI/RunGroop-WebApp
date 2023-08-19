using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        // GET: /<controller>/
        public RaceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Race> races = _context.Races.ToList();
            return View(races );
        }
        public IActionResult Detail(int id)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Race race = _context.Races.Include(a => a.Address).FirstOrDefault(c => c.Id == id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            return View(race );
        }
    }
}

