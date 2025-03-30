using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class TicketPrice
    {
        [Key]
        public int TicketPriceID { get; set; }

        [Required]
        [StringLength(20)]
        public string MealType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
