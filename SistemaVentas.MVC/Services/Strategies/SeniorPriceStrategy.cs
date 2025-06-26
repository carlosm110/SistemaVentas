using SistemaVentas.APIConsumer;
using SistemaVentas.Model;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.MVC.Services.Strategies
{
    public class SeniorPriceStrategy : IPriceStrategy
    {
        private readonly SistemaVentasDBContext _context;

        public SeniorPriceStrategy(SistemaVentasDBContext context)
        {
            _context = context;
        }

        public double Calculate(string routeName, string categoryName, string seatType)
        {
            var route = _context.Routes
                                .AsNoTracking()
                                .FirstOrDefault(r => r.NameRoute == routeName);
            if (route == null)
                throw new ArgumentException($"Ruta «{routeName}» no encontrada.");

            // Precio base de la ruta
            double price = route.Price;

            // Descuento del 20% para tercera edad
            price *= 0.8;

            // Aumento por asiento VIP si aplica
            if (seatType == "VIP")
            {
                price *= 1.5;  // Aumento por asiento VIP
            }

            return price;
        }
    }
}
