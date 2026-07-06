using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymAkhada.Models
{
    public class GymMember
    {
        [Key]
        public int GymMember_ID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Member Type")]
        public string MemberType { get; set; } = "Member";

        [StringLength(20)]
        public string? Gender { get; set; }

        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhar Number must be exactly 12 digits.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Aadhar Number must contain only numbers.")]
        [Display(Name = "Aadhar Number")]
        public string? AadharNumber { get; set; }

        [Required]
        [Range(1, 120)]
        public int? Age { get; set; }

        [Required]
        [Range(1, 500)]
        [Display(Name = "Weight (KGs)")]
        public decimal? WeightKg { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; } = DateTime.Today;

        [Display(Name = "Gym Category")]
        public int? Gym_ID { get; set; }

        [ForeignKey("Gym_ID")]
        public GymCategory? GymCategory { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        [StringLength(250)]
        public string? Remarks { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
