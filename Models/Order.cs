using KauRestaurant.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public string CustomerID { get; set; }  // Links to KauRestaurantUser

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending"; // "Pending", "Completed", or "Cancelled"

        public float TotalPaid { get; set; } = 0;

        public int BreakfastTicketsCount { get; set; } = 0;
        public int LunchTicketsCount { get; set; } = 0;

        [ForeignKey("CustomerID")]
        public virtual KauRestaurantUser Customer { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
