using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Meal
    {
        [Key]
        public int MealID { get; set; }

        [Required]
        public int MenuID { get; set; }

        [Required]
        [StringLength(100)]
        public string MealName { get; set; }

        [Required]
        public string NutritionalInfo { get; set; }

        [Required]
        [StringLength(10)]
        public string MealCategory { get; set; }

        // Navigation property
        [ForeignKey("MenuID")]
        public virtual Menu Menu { get; set; }
    }
}
