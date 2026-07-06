using System.ComponentModel.DataAnnotations;

namespace GymAkhada.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Reset Method")]
        public string ResetMethod { get; set; } = "Email"; // "Email" or "WhatsApp"
    }
}
