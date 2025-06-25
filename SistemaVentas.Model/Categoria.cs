using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SistemaVentas.Model
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required]
        public string Nombre { get; set; }

        public List<Boleto>? Boletos { get; set; }
    }
}
