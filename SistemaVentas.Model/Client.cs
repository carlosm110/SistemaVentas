using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SistemaVentas.Model
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public List<Ticket>? Tickets { get; set; }

    }
}

