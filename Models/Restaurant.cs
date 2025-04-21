using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الموقع")]
        [StringLength(200, ErrorMessage = "يجب ألا يتجاوز الموقع 200 حرف")]
        [Display(Name = "الموقع")]
        public string Location { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رابط الخريطة")]
        [StringLength(255, ErrorMessage = "يجب ألا يتجاوز رابط الخريطة 255 حرف")]
        [Display(Name = "رابط الخريطة")]
        public string GoogleMapsLink { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رقم الاتصال")]
        [Phone(ErrorMessage = "رقم الاتصال غير صالح")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "يرجى إدخال أرقام فقط")]
        [StringLength(20, ErrorMessage = "يجب ألا يتجاوز رقم الاتصال 20 حرف")]
        [Display(Name = "رقم الاتصال")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز البريد الإلكتروني 100 حرف")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يرجى إدخال أيام العمل")]
        [StringLength(100, ErrorMessage = "يجب ألا تتجاوز أيام العمل 100 حرف")]
        [Display(Name = "أيام العمل")]
        public string WorkingDays { get; set; }

        [Required(ErrorMessage = "يرجى إدخال ساعات عمل الإفطار")]
        [StringLength(100, ErrorMessage = "يجب ألا تتجاوز ساعات عمل الإفطار 100 حرف")]
        [Display(Name = "ساعات عمل الإفطار")]
        public string BreakfastHours { get; set; }

        [Required(ErrorMessage = "يرجى إدخال ساعات عمل الغداء")]
        [StringLength(100, ErrorMessage = "يجب ألا تتجاوز ساعات عمل الغداء 100 حرف")]
        [Display(Name = "ساعات عمل الغداء")]
        public string LunchHours { get; set; }

        [StringLength(255, ErrorMessage = "يجب ألا تتجاوز صورة المطعم 255 حرف")]
        [Display(Name = "صورة المطعم")]
        public string Photo { get; set; }

        [Display(Name = "مفتوح")]
        public bool IsOpen { get; set; }
    }
}
