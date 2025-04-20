using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        [Display(Name = "الصورة")]
        public string? Photo { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "الوصف")]
        public string Description { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "الموقع")]
        public string Location { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "ساعات العمل")]
        public string WorkingHours { get; set; }

        [Display(Name = "مفتوح حالياً")]
        public bool IsOpen { get; set; }
    }
}
