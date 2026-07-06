using GymAkhada.Data;
using GymAkhada.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymAkhada.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TournamentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GymAkhada.Services.IEmailService _emailService;

        public TournamentsController(ApplicationDbContext context, GymAkhada.Services.IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var tournaments = await _context.Tournaments.OrderByDescending(t => t.Date).ToListAsync();
            return View(tournaments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                tournament.CreatedAt = DateTime.Now;
                _context.Add(tournament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null) return NotFound();

            return View(tournament);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tournament tournament)
        {
            if (id != tournament.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament != null)
            {
                _context.Tournaments.Remove(tournament);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TournamentExists(int id)
        {
            return _context.Tournaments.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Registrations(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null) return NotFound();

            var registrations = await _context.TournamentRegistrations
                .Include(r => r.GymMember)
                .Include(r => r.GymSubcategory)
                    .ThenInclude(s => s.GymCategory)
                .Where(r => r.TournamentId == id)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();

            ViewBag.Tournament = tournament;
            return View(registrations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRegistration(int registrationId)
        {
            var reg = await _context.TournamentRegistrations
                .Include(r => r.GymMember)
                .Include(r => r.Tournament)
                .Include(r => r.GymSubcategory)
                    .ThenInclude(s => s.GymCategory)
                .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

            if (reg != null)
            {
                reg.Status = "Approved";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registration approved successfully.";

                if (reg.GymMember != null && !string.IsNullOrEmpty(reg.GymMember.Email))
                {
                    string subject = "Tournament Registration APPROVED - AKHADA Gym";
                    string catName = reg.GymSubcategory?.GymCategory?.Gym_categoryName ?? "General";
                    string subName = !string.IsNullOrEmpty(reg.GymSubcategory?.SubcategoryName) ? reg.GymSubcategory.SubcategoryName : "";
                    
                    string body = $"Hello {reg.GymMember.FullName},<br><br>" +
                                  $"Congratulations! Your registration for the tournament <b>{reg.Tournament?.Title}</b> has been <b>APPROVED</b>.<br><br>" +
                                  $"<b>Category:</b> {catName} - {subName}<br>" +
                                  $"<b>Date:</b> {reg.Tournament?.Date.ToString("MMM dd, yyyy - hh:mm tt")}<br>" +
                                  $"<b>Location:</b> {reg.Tournament?.Location}<br><br>" +
                                  $"Please arrive on time and bring your ID proofs.<br><br>" +
                                  $"Thanks,<br>AKHADA Gym Admin";
                    
                    await _emailService.SendEmailAsync(reg.GymMember.Email, subject, body);
                    TempData["Success"] += $" Email notification sent to {reg.GymMember.Email}.";
                }
            }
            return RedirectToAction(nameof(Registrations), new { id = reg?.TournamentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DenyRegistration(int registrationId, string rejectionReason)
        {
            var reg = await _context.TournamentRegistrations
                .Include(r => r.GymMember)
                .Include(r => r.Tournament)
                .Include(r => r.GymSubcategory)
                    .ThenInclude(s => s.GymCategory)
                .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

            if (reg != null)
            {
                reg.Status = "Denied";
                reg.RejectionReason = rejectionReason;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registration denied.";

                if (reg.GymMember != null && !string.IsNullOrEmpty(reg.GymMember.Email))
                {
                    string subject = "Tournament Registration Update - AKHADA Gym";
                    string catName = reg.GymSubcategory?.GymCategory?.Gym_categoryName ?? "General";
                    string subName = !string.IsNullOrEmpty(reg.GymSubcategory?.SubcategoryName) ? reg.GymSubcategory.SubcategoryName : "";
                    
                    string body = $"Hello {reg.GymMember.FullName},<br><br>" +
                                  $"Your registration for the tournament <b>{reg.Tournament?.Title}</b> in category <b>{catName} - {subName}</b> has been updated.<br><br>" +
                                  $"<b>Status:</b> <span style='color:red;'>DENIED</span><br>" +
                                  $"<b>Reason:</b> {rejectionReason}<br><br>" +
                                  $"If you have any questions, please contact the Admin.<br><br>" +
                                  $"Thanks,<br>AKHADA Gym Admin";
                    
                    await _emailService.SendEmailAsync(reg.GymMember.Email, subject, body);
                    TempData["Success"] += $" Email notification sent to {reg.GymMember.Email}.";
                }
            }
            return RedirectToAction(nameof(Registrations), new { id = reg?.TournamentId });
        }
    }
}
