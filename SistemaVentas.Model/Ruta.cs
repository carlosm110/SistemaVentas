using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SistemaVentas.Model
{
    public class Ruta
    {
        [Key]
        public int RutaId { get; set; }

        [Required]
        public string NombreRuta { get; set; }

        public List<Boleto>? Boletos { get; set; }
    }
}
