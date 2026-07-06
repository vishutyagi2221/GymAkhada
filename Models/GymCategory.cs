using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymAkhada.Models
{
    public class GymCategory
    {
        [Key]
        public int Gym_ID { get; set; }

        [Required]
        [Display(Name = "Category Name (e.g. 15-21)")]
        public string Gym_categoryName { get; set; } = string.Empty;

        [Display(Name = "Remarks")]
        public string Gym_Remarks { get; set; } = string.Empty;

        // Navigation property for the subcategories
        public ICollection<GymSubcategory> Subcategories { get; set; } = new List<GymSubcategory>();
    }
}
