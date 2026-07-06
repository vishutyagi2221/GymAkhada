using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymAkhada.Data;
using GymAkhada.Models;

namespace GymAkhada.Controllers
{
    [Authorize]
    public class GymCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GymCategory
        // Routing: /GymCategory or /GymCategory/Index
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.GymCategories.ToListAsync());
        }

        // GET: GymCategory/Create
        // Routing: /GymCategory/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Gym_ID,Gym_categoryName,Gym_Remarks")] GymCategory gymCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Go back to the list
            }
            return View(gymCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.GymCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            bool hasMembers = await _context.GymMembers.AnyAsync(m => m.Gym_ID == id);
            bool hasSubcategories = await _context.GymSubcategories.AnyAsync(s => s.Gym_ID == id);

            if (hasMembers || hasSubcategories)
            {
                TempData["Error"] = "Cannot delete this category because it has members or subcategories assigned to it. Please remove them first.";
                return RedirectToAction(nameof(Index));
            }

            _context.GymCategories.Remove(category);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
