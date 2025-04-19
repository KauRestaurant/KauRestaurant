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

        // Saturday settings
        [Display(Name = "السبت")]
        public bool IsSaturdayOpen { get; set; } = true;

        [Display(Name = "فطور السبت")]
        public bool SaturdayServesBreakfast { get; set; } = false;
        public TimeSpan? SaturdayBreakfastOpenTime { get; set; }
        public TimeSpan? SaturdayBreakfastCloseTime { get; set; }

        [Display(Name = "غداء السبت")]
        public bool SaturdayServesLunch { get; set; } = false;
        public TimeSpan? SaturdayLunchOpenTime { get; set; }
        public TimeSpan? SaturdayLunchCloseTime { get; set; }

        [Display(Name = "عشاء السبت")]
        public bool SaturdayServesDinner { get; set; } = false;
        public TimeSpan? SaturdayDinnerOpenTime { get; set; }
        public TimeSpan? SaturdayDinnerCloseTime { get; set; }

        // Sunday settings
        [Display(Name = "الأحد")]
        public bool IsSundayOpen { get; set; } = true;

        [Display(Name = "فطور الأحد")]
        public bool SundayServesBreakfast { get; set; } = false;
        public TimeSpan? SundayBreakfastOpenTime { get; set; }
        public TimeSpan? SundayBreakfastCloseTime { get; set; }

        [Display(Name = "غداء الأحد")]
        public bool SundayServesLunch { get; set; } = false;
        public TimeSpan? SundayLunchOpenTime { get; set; }
        public TimeSpan? SundayLunchCloseTime { get; set; }

        [Display(Name = "عشاء الأحد")]
        public bool SundayServesDinner { get; set; } = false;
        public TimeSpan? SundayDinnerOpenTime { get; set; }
        public TimeSpan? SundayDinnerCloseTime { get; set; }

        // Monday settings
        [Display(Name = "الاثنين")]
        public bool IsMondayOpen { get; set; } = true;

        [Display(Name = "فطور الاثنين")]
        public bool MondayServesBreakfast { get; set; } = false;
        public TimeSpan? MondayBreakfastOpenTime { get; set; }
        public TimeSpan? MondayBreakfastCloseTime { get; set; }

        [Display(Name = "غداء الاثنين")]
        public bool MondayServesLunch { get; set; } = false;
        public TimeSpan? MondayLunchOpenTime { get; set; }
        public TimeSpan? MondayLunchCloseTime { get; set; }

        [Display(Name = "عشاء الاثنين")]
        public bool MondayServesDinner { get; set; } = false;
        public TimeSpan? MondayDinnerOpenTime { get; set; }
        public TimeSpan? MondayDinnerCloseTime { get; set; }

        // Tuesday settings
        [Display(Name = "الثلاثاء")]
        public bool IsTuesdayOpen { get; set; } = true;

        [Display(Name = "فطور الثلاثاء")]
        public bool TuesdayServesBreakfast { get; set; } = false;
        public TimeSpan? TuesdayBreakfastOpenTime { get; set; }
        public TimeSpan? TuesdayBreakfastCloseTime { get; set; }

        [Display(Name = "غداء الثلاثاء")]
        public bool TuesdayServesLunch { get; set; } = false;
        public TimeSpan? TuesdayLunchOpenTime { get; set; }
        public TimeSpan? TuesdayLunchCloseTime { get; set; }

        [Display(Name = "عشاء الثلاثاء")]
        public bool TuesdayServesDinner { get; set; } = false;
        public TimeSpan? TuesdayDinnerOpenTime { get; set; }
        public TimeSpan? TuesdayDinnerCloseTime { get; set; }

        // Wednesday settings
        [Display(Name = "الأربعاء")]
        public bool IsWednesdayOpen { get; set; } = true;

        [Display(Name = "فطور الأربعاء")]
        public bool WednesdayServesBreakfast { get; set; } = false;
        public TimeSpan? WednesdayBreakfastOpenTime { get; set; }
        public TimeSpan? WednesdayBreakfastCloseTime { get; set; }

        [Display(Name = "غداء الأربعاء")]
        public bool WednesdayServesLunch { get; set; } = false;
        public TimeSpan? WednesdayLunchOpenTime { get; set; }
        public TimeSpan? WednesdayLunchCloseTime { get; set; }

        [Display(Name = "عشاء الأربعاء")]
        public bool WednesdayServesDinner { get; set; } = false;
        public TimeSpan? WednesdayDinnerOpenTime { get; set; }
        public TimeSpan? WednesdayDinnerCloseTime { get; set; }

        // Thursday settings
        [Display(Name = "الخميس")]
        public bool IsThursdayOpen { get; set; } = true;

        [Display(Name = "فطور الخميس")]
        public bool ThursdayServesBreakfast { get; set; } = false;
        public TimeSpan? ThursdayBreakfastOpenTime { get; set; }
        public TimeSpan? ThursdayBreakfastCloseTime { get; set; }

        [Display(Name = "غداء الخميس")]
        public bool ThursdayServesLunch { get; set; } = false;
        public TimeSpan? ThursdayLunchOpenTime { get; set; }
        public TimeSpan? ThursdayLunchCloseTime { get; set; }

        [Display(Name = "عشاء الخميس")]
        public bool ThursdayServesDinner { get; set; } = false;
        public TimeSpan? ThursdayDinnerOpenTime { get; set; }
        public TimeSpan? ThursdayDinnerCloseTime { get; set; }

        // Friday settings
        [Display(Name = "الجمعة")]
        public bool IsFridayOpen { get; set; } = false;

        [Display(Name = "فطور الجمعة")]
        public bool FridayServesBreakfast { get; set; } = false;
        public TimeSpan? FridayBreakfastOpenTime { get; set; }
        public TimeSpan? FridayBreakfastCloseTime { get; set; }

        [Display(Name = "غداء الجمعة")]
        public bool FridayServesLunch { get; set; } = false;
        public TimeSpan? FridayLunchOpenTime { get; set; }
        public TimeSpan? FridayLunchCloseTime { get; set; }

        [Display(Name = "عشاء الجمعة")]
        public bool FridayServesDinner { get; set; } = false;
        public TimeSpan? FridayDinnerOpenTime { get; set; }
        public TimeSpan? FridayDinnerCloseTime { get; set; }

        // Backwards compatibility properties

        [NotMapped]
        public bool ServesBreakfast
        {
            get
            {
                DayOfWeek today = DateTime.Now.DayOfWeek;
                return today switch
                {
                    DayOfWeek.Saturday => SaturdayServesBreakfast,
                    DayOfWeek.Sunday => SundayServesBreakfast,
                    DayOfWeek.Monday => MondayServesBreakfast,
                    DayOfWeek.Tuesday => TuesdayServesBreakfast,
                    DayOfWeek.Wednesday => WednesdayServesBreakfast,
                    DayOfWeek.Thursday => ThursdayServesBreakfast,
                    DayOfWeek.Friday => FridayServesBreakfast,
                    _ => false
                };
            }
            set
            {
                // Legacy setter - update all days
                SaturdayServesBreakfast = value;
                SundayServesBreakfast = value;
                MondayServesBreakfast = value;
                TuesdayServesBreakfast = value;
                WednesdayServesBreakfast = value;
                ThursdayServesBreakfast = value;
                FridayServesBreakfast = value;
            }
        }

        [NotMapped]
        public TimeSpan? BreakfastOpenTime
        {
            get
            {
                DayOfWeek today = DateTime.Now.DayOfWeek;
                return today switch
                {
                    DayOfWeek.Saturday => SaturdayBreakfastOpenTime,
                    DayOfWeek.Sunday => SundayBreakfastOpenTime,
                    DayOfWeek.Monday => MondayBreakfastOpenTime,
                    DayOfWeek.Tuesday => TuesdayBreakfastOpenTime,
                    DayOfWeek.Wednesday => WednesdayBreakfastOpenTime,
                    DayOfWeek.Thursday => ThursdayBreakfastOpenTime,
                    DayOfWeek.Friday => FridayBreakfastOpenTime,
                    _ => null
                };
            }
            set
            {
                // Legacy setter
                SaturdayBreakfastOpenTime = value;
                SundayBreakfastOpenTime = value;
                MondayBreakfastOpenTime = value;
                TuesdayBreakfastOpenTime = value;
                WednesdayBreakfastOpenTime = value;
                ThursdayBreakfastOpenTime = value;
                FridayBreakfastOpenTime = value;
            }
        }

        [NotMapped]
        public TimeSpan? BreakfastCloseTime
        {
            get
            {
                DayOfWeek today = DateTime.Now.DayOfWeek;
                return today switch
                {
                    DayOfWeek.Saturday => SaturdayBreakfastCloseTime,
                    DayOfWeek.Sunday => SundayBreakfastCloseTime,
                    DayOfWeek.Monday => MondayBreakfastCloseTime,
                    DayOfWeek.Tuesday => TuesdayBreakfastCloseTime,
                    DayOfWeek.Wednesday => WednesdayBreakfastCloseTime,
                    DayOfWeek.Thursday => ThursdayBreakfastCloseTime,
                    DayOfWeek.Friday => FridayBreakfastCloseTime,
                    _ => null
                };
            }
            set
            {
                // Legacy setter
                SaturdayBreakfastCloseTime = value;
                SundayBreakfastCloseTime = value;
                MondayBreakfastCloseTime = value;
                TuesdayBreakfastCloseTime = value;
                WednesdayBreakfastCloseTime = value;
                ThursdayBreakfastCloseTime = value;
                FridayBreakfastCloseTime = value;
            }
        }

        [NotMapped]
        public bool ServesLunch
        {
            get
            {
                DayOfWeek today = DateTime.Now.DayOfWeek;
                return today switch
                {
                    DayOfWeek.Saturday => SaturdayServesLunch,
                    DayOfWeek.Sunday => SundayServesLunch,
                    DayOfWeek.Monday => MondayServesLunch,
                    DayOfWeek.Tuesday => TuesdayServesLunch,
                    DayOfWeek.Wednesday => WednesdayServesLunch,
                    DayOfWeek.Thursday => ThursdayServesLunch,
                    DayOfWeek.Friday => FridayServesLunch,
                    _ => false
                };
            }
            set
            {
                // Legacy setter
                SaturdayServesLunch = value;
                SundayServesLunch = value;
                MondayServesLunch = value;
                TuesdayServesLunch = value;
                WednesdayServesLunch = value;
                ThursdayServesLunch = value;
                FridayServesLunch = value;
            }
        }

        [NotMapped]
        public TimeSpan? LunchOpenTime
        {
            get
            {
                DayOfWeek today = DateTime.Now.DayOfWeek;
                return today switch
                {
                    DayOfWeek.Saturday => SaturdayLunchOpenTime,
                    DayOfWeek.Sunday => SundayLunchOpenTime,
                    DayOfWeek.Monday => MondayLunchOpenTime,
                    DayOfWeek.Tuesday => TuesdayLunchOpenTime,
                    DayOfWeek.Wednesday => WednesdayLunchOpenTime,
                    DayOfWeek.Thursday => ThursdayLunchOpenTime,
                    DayOfWeek.Friday => FridayLunchOpenTime,
                    _ => null
                };
            }
            set
            {
                // Legacy setter
                SaturdayLunchOpenTime = value;
                SundayLunchOpenTime = value;
                MondayLunchOpenTime = value;
                TuesdayLunchOpenTime = value;
                WednesdayLunchOpenTime = value;
                ThursdayLunchOpenTime = value;
                FridayLunchOpenTime = value;
            }
        }

        [NotMapped]
        public TimeSpan? LunchCloseTime
        {
            get
            {
                DayOfWeek today = DateTime.Now.DayOfWeek;
                return today switch
                {
                    DayOfWeek.Saturday => SaturdayLunchCloseTime,
                    DayOfWeek.Sunday => SundayLunchCloseTime,
                    DayOfWeek.Monday => MondayLunchCloseTime,
                    DayOfWeek.Tuesday => TuesdayLunchCloseTime,
                    DayOfWeek.Wednesday => WednesdayLunchCloseTime,
                    DayOfWeek.Thursday => ThursdayLunchCloseTime,
                    DayOfWeek.Friday => FridayLunchCloseTime,
                    _ => null
                };
            }
            set
            {
                // Legacy setter
                SaturdayLunchCloseTime = value;
                SundayLunchCloseTime = value;
                MondayLunchCloseTime = value;
                TuesdayLunchCloseTime = value;
                WednesdayLunchCloseTime = value;
                ThursdayLunchCloseTime = value;
                FridayLunchCloseTime = value;
            }
        }

        [NotMapped]
        public bool ServesDinner
        {
            get
            {
                DayOfWeek today = DateTime.Now.DayOfWeek;
                return today switch
                {
                    DayOfWeek.Saturday => SaturdayServesDinner,
                    DayOfWeek.Sunday => SundayServesDinner,
                    DayOfWeek.Monday => MondayServesDinner,
                    DayOfWeek.Tuesday => TuesdayServesDinner,
                    DayOfWeek.Wednesday => WednesdayServesDinner,
                    DayOfWeek.Thursday => ThursdayServesDinner,
                    DayOfWeek.Friday => FridayServesDinner,
                    _ => false
                };
            }
            set
            {
                // Legacy setter
                SaturdayServesDinner = value;
                SundayServesDinner = value;
                MondayServesDinner = value;
                TuesdayServesDinner = value;
                WednesdayServesDinner = value;
                ThursdayServesDinner = value;
                FridayServesDinner = value;
            }
        }

        [NotMapped]
        public TimeSpan? DinnerOpenTime
        {
            get
            {
                DayOfWeek today = DateTime.Now.DayOfWeek;
                return today switch
                {
                    DayOfWeek.Saturday => SaturdayDinnerOpenTime,
                    DayOfWeek.Sunday => SundayDinnerOpenTime,
                    DayOfWeek.Monday => MondayDinnerOpenTime,
                    DayOfWeek.Tuesday => TuesdayDinnerOpenTime,
                    DayOfWeek.Wednesday => WednesdayDinnerOpenTime,
                    DayOfWeek.Thursday => ThursdayDinnerOpenTime,
                    DayOfWeek.Friday => FridayDinnerOpenTime,
                    _ => null
                };
            }
            set
            {
                // Legacy setter
                SaturdayDinnerOpenTime = value;
                SundayDinnerOpenTime = value;
                MondayDinnerOpenTime = value;
                TuesdayDinnerOpenTime = value;
                WednesdayDinnerOpenTime = value;
                ThursdayDinnerOpenTime = value;
                FridayDinnerOpenTime = value;
            }
        }

        [NotMapped]
        public TimeSpan? DinnerCloseTime
        {
            get
            {
                DayOfWeek today = DateTime.Now.DayOfWeek;
                return today switch
                {
                    DayOfWeek.Saturday => SaturdayDinnerCloseTime,
                    DayOfWeek.Sunday => SundayDinnerCloseTime,
                    DayOfWeek.Monday => MondayDinnerCloseTime,
                    DayOfWeek.Tuesday => TuesdayDinnerCloseTime,
                    DayOfWeek.Wednesday => WednesdayDinnerCloseTime,
                    DayOfWeek.Thursday => ThursdayDinnerCloseTime,
                    DayOfWeek.Friday => FridayDinnerCloseTime,
                    _ => null
                };
            }
            set
            {
                // Legacy setter
                SaturdayDinnerCloseTime = value;
                SundayDinnerCloseTime = value;
                MondayDinnerCloseTime = value;
                TuesdayDinnerCloseTime = value;
                WednesdayDinnerCloseTime = value;
                ThursdayDinnerCloseTime = value;
                FridayDinnerCloseTime = value;
            }
        }

        // Helper method to check if a specific day and meal is available
        public bool DoesDayServeMeal(DayOfWeek day, string mealType)
        {
            if (!IsDayOpen(day))
                return false;

            return (day, mealType.ToLower()) switch
            {
                (DayOfWeek.Saturday, "breakfast") => SaturdayServesBreakfast,
                (DayOfWeek.Saturday, "lunch") => SaturdayServesLunch,
                (DayOfWeek.Saturday, "dinner") => SaturdayServesDinner,

                (DayOfWeek.Sunday, "breakfast") => SundayServesBreakfast,
                (DayOfWeek.Sunday, "lunch") => SundayServesLunch,
                (DayOfWeek.Sunday, "dinner") => SundayServesDinner,

                (DayOfWeek.Monday, "breakfast") => MondayServesBreakfast,
                (DayOfWeek.Monday, "lunch") => MondayServesLunch,
                (DayOfWeek.Monday, "dinner") => MondayServesDinner,

                (DayOfWeek.Tuesday, "breakfast") => TuesdayServesBreakfast,
                (DayOfWeek.Tuesday, "lunch") => TuesdayServesLunch,
                (DayOfWeek.Tuesday, "dinner") => TuesdayServesDinner,

                (DayOfWeek.Wednesday, "breakfast") => WednesdayServesBreakfast,
                (DayOfWeek.Wednesday, "lunch") => WednesdayServesLunch,
                (DayOfWeek.Wednesday, "dinner") => WednesdayServesDinner,

                (DayOfWeek.Thursday, "breakfast") => ThursdayServesBreakfast,
                (DayOfWeek.Thursday, "lunch") => ThursdayServesLunch,
                (DayOfWeek.Thursday, "dinner") => ThursdayServesDinner,

                (DayOfWeek.Friday, "breakfast") => FridayServesBreakfast,
                (DayOfWeek.Friday, "lunch") => FridayServesLunch,
                (DayOfWeek.Friday, "dinner") => FridayServesDinner,

                _ => false
            };
        }

        // Helper method to check if a specific day is open
        public bool IsDayOpen(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Saturday => IsSaturdayOpen,
                DayOfWeek.Sunday => IsSundayOpen,
                DayOfWeek.Monday => IsMondayOpen,
                DayOfWeek.Tuesday => IsTuesdayOpen,
                DayOfWeek.Wednesday => IsWednesdayOpen,
                DayOfWeek.Thursday => IsThursdayOpen,
                DayOfWeek.Friday => IsFridayOpen,
                _ => false
            };
        }

        // Add these helper methods to your Restaurant.cs model
        public bool IsDayOpen(string day)
        {
            return (bool)GetType().GetProperty("Is" + day + "Open")?.GetValue(this, null);
        }

        public bool DayServesMeal(string day, string meal)
        {
            return (bool)GetType().GetProperty(day + "Serves" + meal)?.GetValue(this, null);
        }

        public TimeSpan? GetMealOpenTime(string day, string meal)
        {
            return GetType().GetProperty(day + meal + "OpenTime")?.GetValue(this, null) as TimeSpan?;
        }

        public TimeSpan? GetMealCloseTime(string day, string meal)
        {
            return GetType().GetProperty(day + meal + "CloseTime")?.GetValue(this, null) as TimeSpan?;
        }

    }
}
