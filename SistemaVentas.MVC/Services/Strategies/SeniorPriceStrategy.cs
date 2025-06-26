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

            // Descuento 20%
            return route.Price * 0.8;
        }
    }
}
