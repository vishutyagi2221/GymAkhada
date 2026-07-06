using GymAkhada.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GymAkhada.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GymAkhada.Data.ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, GymAkhada.Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // If user is a normal member, redirect to MemberPortal
            if (User.Identity?.IsAuthenticated == true && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "MemberPortal");
            }
            
            var tournaments = await _context.Tournaments.OrderByDescending(t => t.Date).ToListAsync();
            // For everyone else (unauthenticated, or Admin), show the beautiful landing page
            return View(tournaments);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
