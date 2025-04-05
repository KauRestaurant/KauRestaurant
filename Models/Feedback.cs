using KauRestaurant.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        public string? UserID { get; set; }  // Links to KauRestaurantUser

        [Required]
        [StringLength(100)]
        [Display(Name = "الاسم")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "البريد الإلكتروني")]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "الموضوع")]
        public string Subject { get; set; }

        [Required]
        [StringLength(2000)]
        [Display(Name = "الرسالة")]
        public string Text { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("UserID")]
        public virtual KauRestaurantUser User { get; set; }
    }
}
