using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymAkhada.Models
{
    public class TournamentRegistration
    {
        [Key]
        public int RegistrationId { get; set; }

        [Required]
        public int TournamentId { get; set; }
        
        [ForeignKey("TournamentId")]
        public Tournament? Tournament { get; set; }

        [Required]
        public int GymMember_ID { get; set; }
        
        [ForeignKey("GymMember_ID")]
        public GymMember? GymMember { get; set; }

        [Required]
        public int GymSub_ID { get; set; }
        
        [ForeignKey("GymSub_ID")]
        public GymSubcategory? GymSubcategory { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Denied

        [StringLength(500)]
        public string? RejectionReason { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
