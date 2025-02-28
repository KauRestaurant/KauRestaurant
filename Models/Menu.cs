using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }

        [Required]
        [StringLength(10)]
        public string Day { get; set; }

        // Navigation property
        public virtual ICollection<Meal> Meals { get; set; }
    }
}
