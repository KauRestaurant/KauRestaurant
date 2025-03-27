using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KauRestaurant.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        [StringLength(255)]
        public string QRCode { get; set; }

        [Required]
        public string MealType { get; set; }

        public bool IsRedeemed { get; set; } = false;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
    }
}
