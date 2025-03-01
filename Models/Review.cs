using KauRestaurant.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        [Required]
        public string CustomerID { get; set; }

        [Required]
        public int MealID { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        [StringLength(300)]
        public string ReviewText { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [ForeignKey("CustomerID")]
        public virtual KauRestaurantUser Customer { get; set; }

        [ForeignKey("MealID")]
        public virtual Meal Meal { get; set; }
    }
}
