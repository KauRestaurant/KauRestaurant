using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "الموقع")]
        public string Location { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        [Display(Name = "رقم الاتصال")]
        public string ContactNumber { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "ساعات العمل")]
        public string WorkingHours { get; set; }
    }
}
