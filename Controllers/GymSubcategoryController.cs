using GymAkhada.Data;
using GymAkhada.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymAkhada.Controllers
{
    [Authorize]
    public class GymSubcategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymSubcategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var subcategories = await _context.GymSubcategories
                .Include(s => s.GymCategory)
                .OrderBy(s => s.GymCategory!.Gym_categoryName)
                .ThenBy(s => s.GymSub_age)
                .ToListAsync();

            return View(subcategories);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            await PopulateCategoryListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("GymSub_ID,Gym_ID,SubcategoryName,EntryFee,ImportantDetails,GymSub_age,GymWeight")] GymSubcategory gymSubcategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymSubcategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateCategoryListAsync(gymSubcategory.Gym_ID);
            return View(gymSubcategory);
        }

        private async Task PopulateCategoryListAsync(object? selectedCategory = null)
        {
            var categories = await _context.GymCategories
                .AsNoTracking()
                .OrderBy(c => c.Gym_categoryName)
                .ToListAsync();

            ViewBag.Gym_ID = new SelectList(categories, "Gym_ID", "Gym_categoryName", selectedCategory);
            ViewBag.HasCategories = categories.Any();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var gymSubcategory = await _context.GymSubcategories.FindAsync(id);
            if (gymSubcategory == null) return NotFound();

            await PopulateCategoryListAsync(gymSubcategory.Gym_ID);
            return View(gymSubcategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("GymSub_ID,Gym_ID,SubcategoryName,EntryFee,ImportantDetails,GymSub_age,GymWeight")] GymSubcategory gymSubcategory)
        {
            if (id != gymSubcategory.GymSub_ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymSubcategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymSubcategoryExists(gymSubcategory.GymSub_ID)) return NotFound();
                    else throw;
                }
                TempData["Success"] = "Subcategory updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateCategoryListAsync(gymSubcategory.Gym_ID);
            return View(gymSubcategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var gymSubcategory = await _context.GymSubcategories.FindAsync(id);
            if (gymSubcategory != null)
            {
                // Check if there are registrations
                bool hasRegistrations = await _context.TournamentRegistrations.AnyAsync(r => r.GymSub_ID == id);
                if (hasRegistrations)
                {
                    TempData["Error"] = "Cannot delete because there are tournament registrations under this subcategory.";
                    return RedirectToAction(nameof(Index));
                }

                _context.GymSubcategories.Remove(gymSubcategory);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Subcategory deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Subcategory not found.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool GymSubcategoryExists(int id)
        {
            return _context.GymSubcategories.Any(e => e.GymSub_ID == id);
        }
    }
}
