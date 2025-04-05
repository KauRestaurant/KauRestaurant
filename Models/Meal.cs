using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Meal
    {
        [Key]
        public int MealID { get; set; }

        [Required(ErrorMessage = "اسم الوجبة مطلوب")]
        [StringLength(100, ErrorMessage = "يجب أن يكون اسم الوجبة أقل من {1} حرف")]
        public string MealName { get; set; }

        [Required(ErrorMessage = "وصف الوجبة مطلوب")]
        [StringLength(500, ErrorMessage = "يجب أن يكون الوصف أقل من {1} حرف")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "مسار الصورة يجب أن يكون أقل من {1} حرف")]
        public string PicturePath { get; set; }

        [Range(0, 2000, ErrorMessage = "يجب أن تكون السعرات الحرارية بين {1} و {2}")]
        [Required(ErrorMessage = "السعرات الحرارية مطلوبة")]
        public int Calories { get; set; }

        [Range(0, 200, ErrorMessage = "يجب أن تكون كمية البروتين بين {1} و {2} جرام")]
        [Required(ErrorMessage = "كمية البروتين مطلوبة")]
        public int Protein { get; set; }

        [Range(0, 200, ErrorMessage = "يجب أن تكون كمية الكربوهيدرات بين {1} و {2} جرام")]
        [Required(ErrorMessage = "كمية الكربوهيدرات مطلوبة")]
        public int Carbs { get; set; }

        [Range(0, 200, ErrorMessage = "يجب أن تكون كمية الدهون بين {1} و {2} جرام")]
        [Required(ErrorMessage = "كمية الدهون مطلوبة")]
        public int Fat { get; set; }

        [Required(ErrorMessage = "فئة الوجبة مطلوبة")]
        [StringLength(10, ErrorMessage = "فئة الوجبة يجب أن تكون أقل من {1} أحرف")]
        public string MealCategory { get; set; }

        [Required(ErrorMessage = "نوع الوجبة مطلوب")]
        [StringLength(20, ErrorMessage = "نوع الوجبة يجب أن يكون أقل من {1} حرف")]
        public string MealType { get; set; }

        public virtual ICollection<MenuMeal> MenuMeals { get; set; }

        [NotMapped]
        public virtual ICollection<Menu> Menus => MenuMeals?.Select(mm => mm.Menu).ToList();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
