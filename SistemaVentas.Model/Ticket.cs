using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaVentas.Model
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        public double Price { get; set; }
        public bool Delivered { get; set; } = false;

        [ForeignKey("SeatId")]
        public int SeatId { get; set; }

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        [ForeignKey("RouteId")]
        public int RouteId { get; set; }

        public Route? Route{ get; set; }
        public Category? Category { get; set; }
       
        public Seat? Seat { get; set; }
        public Client? Customer { get; set; }
        
    }
}
