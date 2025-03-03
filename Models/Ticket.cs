using KauRestaurant.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public string MealType { get; set; } // "Breakfast", "Lunch", or "Dinner"

        [Required]
        [StringLength(255)]
        public string QRCode { get; set; } = string.Empty;

        public bool IsRedeemed { get; set; } = false;

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
    }
}
