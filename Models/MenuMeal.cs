using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class MenuMeal
    {
        [Key]
        public int MenuMealID { get; set; }

        public int MenuID { get; set; }

        public int MealID { get; set; }

        [ForeignKey("MenuID")]
        public virtual Menu Menu { get; set; }

        [ForeignKey("MealID")]
        public virtual Meal Meal { get; set; }
    }
}
