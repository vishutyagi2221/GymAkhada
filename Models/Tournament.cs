using System.ComponentModel.DataAnnotations;

namespace GymAkhada.Models
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tournament Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(255)]
        public string Location { get; set; } = string.Empty;
        
        public decimal? GoldPrize { get; set; }
        public decimal? SilverPrize { get; set; }
        public decimal? BronzePrize { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
