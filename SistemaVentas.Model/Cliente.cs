using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SistemaVentas.Model
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Email { get; set; }

        public List<Boleto>? Boletos { get; set; }
    }
}
