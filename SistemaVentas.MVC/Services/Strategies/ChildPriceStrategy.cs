using SistemaVentas.APIConsumer; // o el namespace donde esté tu DbContext
using SistemaVentas.Model;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentas.MVC.Services.Strategies
{
    public class ChildPriceStrategy : IPriceStrategy
    {
        private readonly SistemaVentasDBContext _context;

        public ChildPriceStrategy(SistemaVentasDBContext context)
        {
            _context = context;
        }

        public double Calculate(string routeName, string categoryName, string seatType)
        {
            // Search route by name
            var route = _context.Routes
                                .AsNoTracking()
                                .FirstOrDefault(r => r.NameRoute == routeName);
            if (route == null)
                throw new ArgumentException($"Ruta «{routeName}» no encontrada.");

            // Descuento 50%
            return route.Price * 0.5;
        }
    }
}