using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }

        [Required]
        [StringLength(10)]
        public string Day { get; set; }

        // Updated navigation properties
        public virtual ICollection<MenuMeal> MenuMeals { get; set; }

        [NotMapped]
        public virtual ICollection<Meal> Meals => MenuMeals?.Select(mm => mm.Meal).ToList();
    }
}
