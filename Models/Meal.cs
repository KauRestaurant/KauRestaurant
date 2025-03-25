using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Meal
    {
        [Key]
        public int MealID { get; set; }

        [Required]
        [StringLength(100)]
        public string MealName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(255)]
        public string PicturePath { get; set; }

        [Range(0, 2000)]
        public int Calories { get; set; }

        [Range(0, 200)]
        public int Protein { get; set; }

        [Range(0, 200)]
        public int Carbs { get; set; }

        [Range(0, 200)]
        public int Fat { get; set; }

        [Required]
        [StringLength(10)]
        public string MealCategory { get; set; }

        [Required]
        [StringLength(20)]
        public string MealType { get; set; }

        public virtual ICollection<MenuMeal> MenuMeals { get; set; }

        [NotMapped]
        public virtual ICollection<Menu> Menus => MenuMeals?.Select(mm => mm.Menu).ToList();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
