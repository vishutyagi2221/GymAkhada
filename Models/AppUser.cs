using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymAkhada.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Member"; // "Admin" or "Member"

        // Nullable foreign key. If a user is a Member, this links to their GymMember record.
        public int? GymMember_ID { get; set; }

        [ForeignKey("GymMember_ID")]
        public GymMember? GymMember { get; set; }

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
    }
}
