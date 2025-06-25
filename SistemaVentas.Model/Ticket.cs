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

        [ForeignKey("SeatId")]
        public int AsientoId { get; set; }

        [ForeignKey("ClienteId")]
        public int ClienteId { get; set; }

        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }

        [ForeignKey("RutaId")]
        public int RutaId { get; set; }

        public Ruta? Ruta { get; set; }
        public Categoria? Categoria { get; set; }
       
        public Asiento? Asiento { get; set; }
        public Cliente? Cliente { get; set; }
        
    }
}
