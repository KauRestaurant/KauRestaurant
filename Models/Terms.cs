using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Models
{
    public class Terms
    {
        [Key]
        public int TermID { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يزيد العنوان عن 100 حرف")]
        public string Title { get; set; }

        [Required(ErrorMessage = "المحتوى مطلوب")]
        [StringLength(2000, ErrorMessage = "يجب ألا يزيد المحتوى عن 2000 حرف")]
        public string Content { get; set; }

        [Required(ErrorMessage = "تاريخ التحديث مطلوب")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public int DisplayOrder { get; set; } = 0;
    }
}
