using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SistemaVentas.Model
{
    public class Asiento
    {
        [Key]
        public int AsientoId { get; set; }

        [Required]
        public string Tipo { get; set; }

        public List<Boleto>? Boletos { get; set; }
    }
}
