using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SistemaVentas.Model
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        [Required]
        public string NameRoute { get; set; }

        public List<Ticket>? Tickets { get; set; }
    }
}
