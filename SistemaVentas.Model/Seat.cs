using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SistemaVentas.Model
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        [Required]
        public string Type { get; set; }

        public List<Ticket>? TIckets { get; set; }
    }
}
