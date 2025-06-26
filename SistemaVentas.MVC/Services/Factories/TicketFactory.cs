using SistemaVentas.Model;
using SistemaVentas.MVC.Services.Factories.SistemaVentas.MVC.Services.Factories;

namespace SistemaVentas.MVC.Services.Factories
{
    public class TicketFactory : IBoletoFactory
    {
        public Ticket CreateTicket(
            string routeName,
            string categoryName,
            string seatType,
            double price,
            Client customer)
        {
            // Here we would typically retrieve the Route, Category, and Seat from a database
            // and assign them to the Ticket.
            return new Ticket
            {
                Route = new Model.Route { NameRoute = routeName },
                Category = new Category { Name = categoryName },
                Seat = new Seat { Type = seatType },
                Price = price,
                Customer = customer
            };
        }
    }
}