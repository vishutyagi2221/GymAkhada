using GymAkhada.Data;
using GymAkhada.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymAkhada.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? date)
        {
            // Get all members and attendance for target date
            var targetDate = date?.Date ?? DateTime.Now.Date;
            var members = await _context.GymMembers.ToListAsync();
            var todaysAttendance = await _context.Attendances
                .Where(a => a.Date == targetDate)
                .ToDictionaryAsync(a => a.GymMember_ID);

            ViewBag.TodaysAttendance = todaysAttendance;
            ViewBag.Today = targetDate;

            return View(members);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Mark(int memberId, bool isPresent, DateTime date)
        {
            var targetDate = date.Date;
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.GymMember_ID == memberId && a.Date == targetDate);

            if (attendance == null)
            {
                attendance = new Attendance
                {
                    GymMember_ID = memberId,
                    Date = targetDate,
                    IsPresent = isPresent,
                    IsApproved = true
                };
                _context.Attendances.Add(attendance);
            }
            else
            {
                attendance.IsPresent = isPresent;
                attendance.IsApproved = true;
                _context.Attendances.Update(attendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { date = targetDate.ToString("yyyy-MM-dd") });
        }
    }
}
