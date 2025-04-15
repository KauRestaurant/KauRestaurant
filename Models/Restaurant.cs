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
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "يجب أن يتكون رقم الهاتف من أرقام فقط")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز البريد الإلكتروني 100 حرف")]
        [EmailAddress(ErrorMessage = "يرجى إدخال بريد إلكتروني صحيح")]
        public string Email { get; set; }

        // Working hours for different meal periods
        // Models/Restaurant.cs - Update these properties:

        // Breakfast working hours
        public TimeSpan? BreakfastOpenTime { get; set; }
        public TimeSpan? BreakfastCloseTime { get; set; }
        public bool ServesBreakfast { get; set; } = false;

        // Lunch working hours
        public TimeSpan? LunchOpenTime { get; set; }
        public TimeSpan? LunchCloseTime { get; set; }
        public bool ServesLunch { get; set; } = false;

        // Dinner working hours
        public TimeSpan? DinnerOpenTime { get; set; }
        public TimeSpan? DinnerCloseTime { get; set; }
        public bool ServesDinner { get; set; } = false;


        // Days of operation (comma-separated string like "Mon,Tue,Wed,Thu,Fri")
        [Required(ErrorMessage = "أيام العمل مطلوبة")]
        [StringLength(100, ErrorMessage = "يجب ألا تتجاوز أيام العمل 100 حرف")]
        public string DaysOpen { get; set; }
    }
}
