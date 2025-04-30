using KauRestaurant.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        public string CustomerID { get; set; }

        public int MealID { get; set; }

        public DateTime ReviewDate { get; set; }

        [StringLength(300, ErrorMessage = "يجب أن يكون نص التقييم أقل من {1} حرف")]
        public string ReviewText { get; set; }

        [Required(ErrorMessage = "التقييم مطلوب")]
        [Range(1, 5, ErrorMessage = "يجب أن يكون التقييم بين {1} و {2} نجوم")]
        public int Rating { get; set; }

        [ForeignKey("CustomerID")]
        public virtual KauRestaurantUser Customer { get; set; }

        [ForeignKey("MealID")]
        public virtual Meal Meal { get; set; }
    }
}
