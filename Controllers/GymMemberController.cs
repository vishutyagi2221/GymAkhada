using GymAkhada.Data;
using GymAkhada.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymAkhada.Controllers
{
    [Authorize]
    public class GymMemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymMemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _context.GymMembers
                .Include(m => m.GymCategory)
                .OrderByDescending(m => m.JoiningDate)
                .ThenBy(m => m.FullName)
                .ToListAsync();

            return View(members);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            await PopulateFormListsAsync();
            return View(new GymMember
            {
                JoiningDate = DateTime.Today,
                IsActive = true,
                MemberType = "Member"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("GymMember_ID,FullName,MobileNumber,Gender,Age,WeightKg,JoiningDate,Gym_ID,Address,Remarks,IsActive")] GymMember gymMember)
        {
            gymMember.MemberType = "Member";

            if (ModelState.IsValid)
            {
                _context.Add(gymMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateFormListsAsync(gymMember.Gym_ID, gymMember.Gender);
            return View(gymMember);
        }

        private async Task PopulateFormListsAsync(object? selectedCategory = null, string? selectedGender = null)
        {
            var categories = await _context.GymCategories
                .AsNoTracking()
                .OrderBy(c => c.Gym_categoryName)
                .ToListAsync();

            ViewBag.Gym_ID = new SelectList(categories, "Gym_ID", "Gym_categoryName", selectedCategory);
            ViewBag.Genders = new SelectList(new[] { "Male", "Female", "Other" }, selectedGender);
        }
    }
}
