using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymAkhada.Models
{
    public class GymSubcategory
    {
        [Key]
        public int GymSub_ID { get; set; }

        [Required]
        [Display(Name = "Gym Category")]
        public int Gym_ID { get; set; }

        [ForeignKey("Gym_ID")]
        public GymCategory? GymCategory { get; set; }

        [Display(Name = "Subcategory Name (e.g. 50-60 KGs)")]
        [StringLength(100)]
        public string? SubcategoryName { get; set; }

        [Display(Name = "Age Criteria (Optional)")]
        public int? GymSub_age { get; set; }

        [Display(Name = "Weight KGs (Optional)")]
        public decimal? GymWeight { get; set; }

        [Display(Name = "Entry Fee (₹)")]
        public decimal? EntryFee { get; set; }

        [Display(Name = "Important Details")]
        [StringLength(500)]
        public string? ImportantDetails { get; set; }
    }
}
