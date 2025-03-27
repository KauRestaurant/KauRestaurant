using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Models
{
    public class FAQ
    {
        [Key]
        public int FAQID { get; set; }

        [Required]
        [StringLength(500)]
        public string Question { get; set; }

        [Required]
        [StringLength(2000)]
        public string Answer { get; set; }

        public int DisplayOrder { get; set; } = 0;
    }
}
