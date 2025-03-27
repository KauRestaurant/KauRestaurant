using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Models
{
    public class SocialMedia
    {
        [Key]
        public int SocialMediaID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Icon { get; set; } // Example: "bi-facebook", "bi-twitter", etc.

        [StringLength(255)]
        public string? Link { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; } = 0;
    }
}
