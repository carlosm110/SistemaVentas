using SistemaVentas.APIConsumer;
using SistemaVentas.Model;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.MVC.Services.Strategies
{
    public class AdultPriceStrategy : IPriceStrategy
    {
        private readonly SistemaVentasDBContext _context;

        public AdultPriceStrategy(SistemaVentasDBContext context)
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

            // Aumento por asiento VIP si aplica
            if (seatType == "VIP")
            {
                price *= 1.5;  // 50% más caro para asiento VIP
            }

            // Lógica para categoría (adulto)
            if (categoryName == "Adulto")
            {
                // Se queda con el precio base para adultos
            }

            return price;
        }
    }
}
