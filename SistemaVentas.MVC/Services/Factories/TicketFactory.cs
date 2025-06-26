using SistemaVentas.Model;
using SistemaVentas.MVC.Services.Factories.SistemaVentas.MVC.Services.Factories;

namespace SistemaVentas.MVC.Services.Factories
{
    public class TicketFactory : IBoletoFactory
    {
        private readonly SistemaVentasDBContext _context;

        public TicketFactory(SistemaVentasDBContext context)
        {
            _context = context;
        }

        public Ticket CreateTicket(
            string routeName,
            string categoryName,
            string seatType,
            double price,
            Client customer)
        {
            // Consultar la base de datos para obtener la Route, Category y Seat ya existentes
            var route = _context.Routes.FirstOrDefault(r => r.NameRoute == routeName);
            var category = _context.Categories.FirstOrDefault(c => c.Name == categoryName);
            var seat = _context.Seats.FirstOrDefault(s => s.Type == seatType);

            if (route == null || category == null || seat == null)
            {
                throw new InvalidOperationException("Route, Category or Seat not found in the database.");
            }

            // Crear el ticket usando las instancias obtenidas de la base de datos
            return new Ticket
            {
                Route = route,
                Category = category,
                Seat = seat,
                Price = price,
                Customer = customer
            };
        }
    }
}
