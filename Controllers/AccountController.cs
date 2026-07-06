using GymAkhada.Data;
using GymAkhada.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GymAkhada.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GymAkhada.Services.IEmailService _emailService;
        private readonly GymAkhada.Services.IWhatsAppService _whatsAppService;

        public AccountController(ApplicationDbContext context, GymAkhada.Services.IEmailService emailService, GymAkhada.Services.IWhatsAppService whatsAppService)
        {
            _context = context;
            _emailService = emailService;
            _whatsAppService = whatsAppService;
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(model.Password);
                var user = await _context.AppUsers
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.PasswordHash == hashedPassword);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("UserId", user.UserId.ToString())
                    };

                    if (user.GymMember_ID.HasValue)
                    {
                        claims.Add(new Claim("GymMemberId", user.GymMember_ID.Value.ToString()));
                    }

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Redirect based on role
                    if (user.Role == "Admin")
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "MemberPortal");
                    }
                }
                
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Change Password Feature
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            var user = await _context.AppUsers.FindAsync(userId);

            if (user != null && user.PasswordHash == HashPassword(currentPassword))
            {
                user.PasswordHash = HashPassword(newPassword);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Password updated successfully!";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Incorrect current password.");
            return View();
        }
        // Register Feature
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.AppUsers.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(model);
                }

                // Create a basic GymMember record for the new user
                var newMember = new GymMember
                {
                    FullName = model.FullName,
                    MobileNumber = model.ContactNumber,
                    Email = model.Email,
                    Address = "Not Provided",
                    IsActive = true,
                    JoiningDate = DateTime.Now,
                    MemberType = "Member",
                    Age = model.Age,
                    WeightKg = 60 // Default weight
                };

                _context.GymMembers.Add(newMember);
                await _context.SaveChangesAsync(); // Save to get the generated GymMember_ID

                var newUser = new AppUser
                {
                    Username = model.Username,
                    PasswordHash = HashPassword(model.Password),
                    Role = "Member",
                    GymMember_ID = newMember.GymMember_ID
                };

                _context.AppUsers.Add(newUser);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.AppUsers
                    .Include(u => u.GymMember)
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user == null || user.GymMember == null)
                {
                    ModelState.AddModelError("Username", "No user found with that username or missing member profile.");
                    return View(model);
                }

                // Generate token
                user.ResetToken = Guid.NewGuid().ToString();
                user.ResetTokenExpiry = DateTime.Now.AddHours(1);
                await _context.SaveChangesAsync();

                var resetLink = Url.Action("ResetPassword", "Account", new { token = user.ResetToken }, Request.Scheme);
                
                string message = $"Your password reset link is: {resetLink}\nThis link will expire in 1 hour.";

                if (model.ResetMethod == "Email" && !string.IsNullOrEmpty(user.GymMember.Email))
                {
                    await _emailService.SendEmailAsync(user.GymMember.Email, "AKHADA - Password Reset", message);
                    TempData["SuccessMessage"] = "Password reset link has been sent to your registered email.";
                }
                else if (model.ResetMethod == "WhatsApp" && !string.IsNullOrEmpty(user.GymMember.MobileNumber))
                {
                    await _whatsAppService.SendWhatsAppMessageAsync(user.GymMember.MobileNumber, message);
                    TempData["SuccessMessage"] = "Password reset link has been sent to your registered WhatsApp number.";
                }
                else
                {
                    ModelState.AddModelError("", "Selected contact method is missing in your profile.");
                    return View(model);
                }

                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login");
            
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid or expired password reset token.";
                return RedirectToAction("Login");
            }

            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.ResetToken == model.Token && u.ResetTokenExpiry > DateTime.Now);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Invalid or expired password reset token.";
                    return RedirectToAction("Login");
                }

                user.PasswordHash = HashPassword(model.NewPassword);
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Password has been successfully reset! Please login.";
                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}
