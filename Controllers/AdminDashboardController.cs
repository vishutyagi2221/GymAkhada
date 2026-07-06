using GymAkhada.Models;
using GymAkhada.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GymAkhada.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Category Chart Data
            var categoryData = await _context.GymMembers
                .Include(m => m.GymCategory)
                .GroupBy(m => m.GymCategory.Gym_categoryName)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.CategoryLabels = categoryData.Select(c => c.Category).ToList();
            ViewBag.CategoryCounts = categoryData.Select(c => c.Count).ToList();

            // Revenue Data (Last 6 Months)
            var sixMonthsAgo = DateTime.Now.AddMonths(-5);
            var revenueData = await _context.FeePayments
                .Where(f => f.PaymentDate >= new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1))
                .GroupBy(f => new { f.PaymentDate.Year, f.PaymentDate.Month })
                .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Total = g.Sum(f => f.Amount) })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToListAsync();

            var monthLabels = revenueData.Select(r => new DateTime(r.Year, r.Month, 1).ToString("MMM")).ToList();
            var revenueAmounts = revenueData.Select(r => r.Total).ToList();

            ViewBag.MonthLabels = monthLabels;
            ViewBag.RevenueAmounts = revenueAmounts;
            ViewBag.TotalRevenue = revenueAmounts.Sum();

            // Expiry Alerts (Expiring within 7 days)
            var nextWeek = DateTime.Now.AddDays(7);
            var expiryAlerts = await _context.FeePayments
                .Include(f => f.GymMember)
                .Where(f => f.ValidTill <= nextWeek && f.ValidTill >= DateTime.Now.AddDays(-30))
                .OrderBy(f => f.ValidTill)
                .ToListAsync();
            
            ViewBag.ExpiryAlerts = expiryAlerts;
            
            // Total Members
            ViewBag.TotalMembers = await _context.GymMembers.CountAsync();
            
            return View();
        }
    }
}
