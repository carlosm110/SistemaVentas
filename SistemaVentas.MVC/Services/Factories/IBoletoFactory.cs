using SistemaVentas.Model;

namespace SistemaVentas.MVC.Services.Factories
{
    namespace SistemaVentas.MVC.Services.Factories
    {
        public interface IBoletoFactory
        {
            Ticket CreateTicket(string routeName, string categoryName, string seatType, double price, Client customer);
        }
    }
}
