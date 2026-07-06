using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymAkhada.Models
{
    public class FeePayment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [Display(Name = "Member")]
        public int GymMember_ID { get; set; }

        [ForeignKey("GymMember_ID")]
        public GymMember? GymMember { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        [Display(Name = "Amount (₹)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Valid Till (Expiry)")]
        public DateTime ValidTill { get; set; } = DateTime.Now.AddMonths(1);

        [Required]
        [MaxLength(50)]
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; } = "Cash"; // Cash, UPI, Card

        [MaxLength(250)]
        public string? Remarks { get; set; }
    }
}
