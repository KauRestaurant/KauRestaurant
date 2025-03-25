using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Store the image path for the restaurant
        [StringLength(255)]
        public string PhotoPath { get; set; }

        // Google Maps location URL
        [Required]
        [StringLength(500)]
        public string LocationUrl { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        // Contact information
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        // Working hours for different meal periods
        // Breakfast working hours
        public TimeSpan? BreakfastOpenTime { get; set; }
        public TimeSpan? BreakfastCloseTime { get; set; }
        public bool ServesBreakfast { get; set; } = true;

        // Lunch working hours
        public TimeSpan? LunchOpenTime { get; set; }
        public TimeSpan? LunchCloseTime { get; set; }
        public bool ServesLunch { get; set; } = true;

        // Dinner working hours
        public TimeSpan? DinnerOpenTime { get; set; }
        public TimeSpan? DinnerCloseTime { get; set; }
        public bool ServesDinner { get; set; } = true;

        // Days of operation (comma-separated string like "Mon,Tue,Wed,Thu,Fri")
        [StringLength(100)]
        public string DaysOpen { get; set; }
    }
}
