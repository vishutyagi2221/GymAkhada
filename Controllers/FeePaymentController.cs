using GymAkhada.Data;
using GymAkhada.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymAkhada.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeePaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GymAkhada.Services.IEmailService _emailService;
        private readonly GymAkhada.Services.IWhatsAppService _whatsappService;

        public FeePaymentController(ApplicationDbContext context, GymAkhada.Services.IEmailService emailService, GymAkhada.Services.IWhatsAppService whatsappService)
        {
            _context = context;
            _emailService = emailService;
            _whatsappService = whatsappService;
        }

        public async Task<IActionResult> Index()
        {
            var feePayments = await _context.FeePayments
                .Include(f => f.GymMember)
                .OrderByDescending(f => f.PaymentDate)
                .ToListAsync();

            return View(feePayments);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateMembersDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,GymMember_ID,Amount,PaymentDate,ValidTill,PaymentMode,Remarks")] FeePayment feePayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feePayment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fee recorded successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateMembersDropdown(feePayment.GymMember_ID);
            return View(feePayment);
        }

        private async Task PopulateMembersDropdown(object? selectedMember = null)
        {
            var members = await _context.GymMembers
                .AsNoTracking()
                .OrderBy(m => m.FullName)
                .Select(m => new { m.GymMember_ID, m.FullName })
                .ToListAsync();

            ViewBag.GymMember_ID = new SelectList(members, "GymMember_ID", "FullName", selectedMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailReceipt(int paymentId)
        {
            var payment = await _context.FeePayments
                .Include(p => p.GymMember)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment != null && payment.GymMember != null && !string.IsNullOrEmpty(payment.GymMember.Email))
            {
                string subject = "AKHADA Gym - Fee Receipt";
                string body = $"Hello {payment.GymMember.FullName},<br><br>Your fee payment of <b>Rs.{payment.Amount}</b> has been received successfully.<br>Valid till: <b>{payment.ValidTill:dd-MMM-yyyy}</b>.<br><br>Thanks,<br>AKHADA Gym";
                
                await _emailService.SendEmailAsync(payment.GymMember.Email, subject, body);
                TempData["SuccessMessage"] = $"Email receipt sent successfully to {payment.GymMember.Email}.";
            }
            else
            {
                TempData["ErrorMessage"] = "Could not send email. Member email address is missing.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
