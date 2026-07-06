using GymAkhada.Data;
using GymAkhada.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GymAkhada.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberPortalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberPortalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var memberIdClaim = User.FindFirst("GymMemberId")?.Value;
            if (string.IsNullOrEmpty(memberIdClaim))
            {
                return RedirectToAction("Login", "Account");
            }

            var gymMemberId = int.Parse(memberIdClaim);
            var memberInfo = await _context.GymMembers
                .Include(m => m.GymCategory)
                .FirstOrDefaultAsync(m => m.GymMember_ID == gymMemberId);

            if (memberInfo == null)
            {
                return NotFound();
            }

            var today = DateTime.Now.Date;
            var todaysAttendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.GymMember_ID == gymMemberId && a.Date == today);
            
            ViewBag.TodaysAttendance = todaysAttendance;
            ViewBag.Tournaments = await _context.Tournaments.OrderByDescending(t => t.Date).ToListAsync();

            var myRegistrations = await _context.TournamentRegistrations
                .Include(r => r.Tournament)
                .Include(r => r.GymSubcategory)
                    .ThenInclude(s => s.GymCategory)
                .Where(r => r.GymMember_ID == gymMemberId && r.Status == "Approved")
                .OrderByDescending(r => r.Tournament!.Date)
                .ToListAsync();
            
            ViewBag.MyRegistrations = myRegistrations;

            return View(memberInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAttendance()
        {
            var memberIdClaim = User.FindFirst("GymMemberId")?.Value;
            if (!string.IsNullOrEmpty(memberIdClaim))
            {
                var gymMemberId = int.Parse(memberIdClaim);
                var today = DateTime.Now.Date;
                
                var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.GymMember_ID == gymMemberId && a.Date == today);

                if (attendance == null)
                {
                    attendance = new Attendance
                    {
                        GymMember_ID = gymMemberId,
                        Date = today,
                        IsPresent = true,
                        IsApproved = false
                    };
                    _context.Attendances.Add(attendance);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> TournamentRegistration(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null) return NotFound();

            var categories = await _context.GymCategories
                .Include(c => c.Subcategories)
                .OrderBy(c => c.Gym_categoryName)
                .ToListAsync();

            var memberIdClaim = User.FindFirst("GymMemberId")?.Value;
            int memberId = !string.IsNullOrEmpty(memberIdClaim) ? int.Parse(memberIdClaim) : 0;

            var existingRegistrations = await _context.TournamentRegistrations
                .Include(r => r.GymSubcategory)
                .Where(r => r.TournamentId == id && r.GymMember_ID == memberId)
                .ToListAsync();

            ViewBag.Tournament = tournament;
            ViewBag.Categories = categories;
            ViewBag.ExistingRegistrations = existingRegistrations;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitRegistration(int tournamentId, int gymSubId)
        {
            var memberIdClaim = User.FindFirst("GymMemberId")?.Value;
            if (string.IsNullOrEmpty(memberIdClaim)) return RedirectToAction("Login", "Account");
            int memberId = int.Parse(memberIdClaim);

            var existingRegistrations = await _context.TournamentRegistrations
                .Include(r => r.GymSubcategory)
                .ThenInclude(s => s.GymCategory)
                .Where(r => r.TournamentId == tournamentId && r.GymMember_ID == memberId)
                .ToListAsync();

            var selectedSub = await _context.GymSubcategories
                .Include(s => s.GymCategory)
                .FirstOrDefaultAsync(s => s.GymSub_ID == gymSubId);

            if (selectedSub == null) return NotFound();

            // Rules: Max 2 registrations. One particular, one "Open for All".
            if (existingRegistrations.Count >= 2)
            {
                TempData["Error"] = "You can only register for a maximum of 2 categories (One specific + One Open for All).";
                return RedirectToAction(nameof(TournamentRegistration), new { id = tournamentId });
            }

            // Check if already registered for this specific subcategory
            if (existingRegistrations.Any(r => r.GymSub_ID == gymSubId))
            {
                TempData["Error"] = "You are already registered for this category.";
                return RedirectToAction(nameof(TournamentRegistration), new { id = tournamentId });
            }

            bool isOpenForAll = selectedSub.GymCategory?.Gym_categoryName == "Open for All" || 
                                (selectedSub.SubcategoryName != null && selectedSub.SubcategoryName.Contains("Open", StringComparison.OrdinalIgnoreCase));

            // If they already have 1 registration, the second MUST be "Open for All" (or the first was Open and this is specific)
            if (existingRegistrations.Count == 1)
            {
                var existing = existingRegistrations.First();
                bool existingIsOpen = existing.GymSubcategory?.GymCategory?.Gym_categoryName == "Open for All" || 
                                      (existing.GymSubcategory?.SubcategoryName != null && existing.GymSubcategory.SubcategoryName.Contains("Open", StringComparison.OrdinalIgnoreCase));
                
                if (!isOpenForAll && !existingIsOpen)
                {
                    TempData["Error"] = "You can only select one specific category. The second category must be 'Open for All'.";
                    return RedirectToAction(nameof(TournamentRegistration), new { id = tournamentId });
                }
                
                if (isOpenForAll && existingIsOpen)
                {
                    TempData["Error"] = "You can only select 'Open for All' once.";
                    return RedirectToAction(nameof(TournamentRegistration), new { id = tournamentId });
                }
            }

            var registration = new TournamentRegistration
            {
                TournamentId = tournamentId,
                GymMember_ID = memberId,
                GymSub_ID = gymSubId,
                Status = "Pending",
                RegistrationDate = DateTime.Now
            };

            _context.TournamentRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Successfully applied for the tournament category!";
            return RedirectToAction(nameof(TournamentRegistration), new { id = tournamentId });
        }
    }
}
