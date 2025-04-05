using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        [Required(ErrorMessage = "اسم المطعم مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم المطعم 100 حرف")]
        public string Name { get; set; }

        [Required(ErrorMessage = "وصف المطعم مطلوب")]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز وصف المطعم 500 حرف")]
        public string Description { get; set; }

        // Store the image path for the restaurant
        [Required(ErrorMessage = "مسار الصورة مطلوب")]
        [StringLength(255, ErrorMessage = "يجب ألا يتجاوز مسار الصورة 255 حرف")]
        public string PhotoPath { get; set; }

        // Google Maps location URL
        [Required(ErrorMessage = "رابط الموقع على الخريطة مطلوب")]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز رابط الموقع 500 حرف")]
        public string LocationUrl { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        [StringLength(200, ErrorMessage = "يجب ألا يتجاوز العنوان 200 حرف")]
        public string Address { get; set; }

        // Contact information
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(20, ErrorMessage = "يجب ألا يتجاوز رقم الهاتف 20 رقماً")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز البريد الإلكتروني 100 حرف")]
        [EmailAddress(ErrorMessage = "يرجى إدخال بريد إلكتروني صحيح")]
        public string Email { get; set; }

        // Working hours for different meal periods
        // Breakfast working hours
        [Required(ErrorMessage = "وقت فتح الإفطار مطلوب")]
        public TimeSpan? BreakfastOpenTime { get; set; }
        [Required(ErrorMessage = "وقت إغلاق الإفطار مطلوب")]
        public TimeSpan? BreakfastCloseTime { get; set; }
        public bool ServesBreakfast { get; set; } = true;

        // Lunch working hours
        [Required(ErrorMessage = "وقت فتح الغداء مطلوب")]
        public TimeSpan? LunchOpenTime { get; set; }
        [Required(ErrorMessage = "وقت إغلاق الغداء مطلوب")]
        public TimeSpan? LunchCloseTime { get; set; }
        public bool ServesLunch { get; set; } = true;

        // Dinner working hours
        [Required(ErrorMessage = "وقت فتح العشاء مطلوب")]
        public TimeSpan? DinnerOpenTime { get; set; }
        [Required(ErrorMessage = "وقت إغلاق العشاء مطلوب")]
        public TimeSpan? DinnerCloseTime { get; set; }
        public bool ServesDinner { get; set; } = true;

        // Days of operation (comma-separated string like "Mon,Tue,Wed,Thu,Fri")
        [Required(ErrorMessage = "أيام العمل مطلوبة")]
        [StringLength(100, ErrorMessage = "يجب ألا تتجاوز أيام العمل 100 حرف")]
        public string DaysOpen { get; set; }
    }
}
