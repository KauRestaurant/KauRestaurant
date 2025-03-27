using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Models
{
    public class Terms
    {
        [Key]
        public int TermID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public int DisplayOrder { get; set; } = 0;
    }
}
