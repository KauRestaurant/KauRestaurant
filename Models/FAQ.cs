using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Models
{
    public class FAQ
    {
        [Key]
        public int FAQID { get; set; }

        [Required(ErrorMessage = "السؤال مطلوب")]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز طول السؤال 500 حرف")]
        public string Question { get; set; }

        [Required(ErrorMessage = "الإجابة مطلوبة")]
        [StringLength(2000, ErrorMessage = "يجب ألا يتجاوز طول الإجابة 2000 حرف")]
        public string Answer { get; set; }

        public int DisplayOrder { get; set; } = 0;
    }
}
