using Microsoft.EntityFrameworkCore;
using SistemaVentas.Model;
using SistemaVentas.MVC.Services.Factories;
using SistemaVentas.MVC.Services.Factories.SistemaVentas.MVC.Services.Factories;
using SistemaVentas.MVC.Services.Observers;
using SistemaVentas.MVC.Services.Strategies;

namespace SistemaVentas.MVC.Services.Business
{
    public class TicketService
    {
        private readonly IBoletoFactory _factory;
        private readonly PriceCalculator _calculator;
        private readonly List<IObserver> _observers = new();
        private readonly SistemaVentasDBContext _context;

        // Constructor
        public TicketService(IBoletoFactory factory, PriceCalculator calculator, SistemaVentasDBContext context)
        {
            _factory = factory;
            _calculator = calculator;
            _context = context;
        }

        // Método para registrar observadores
        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        // Método privado para notificar a todos los observadores
        private void NotifyAll(Ticket ticket)
        {
            foreach (var observer in _observers)
            {
                observer.Update(ticket);
            }
        }

        // Método para calcular el precio basado en la ruta, categoría y tipo de asiento
        public double CalculatePrice(string routeName, string categoryName, string seatType, IPriceStrategy strategy)
        {
            // Configuramos la estrategia de precio
            _calculator.SetStrategy(strategy);

            // Calculamos el precio usando la estrategia seleccionada
            return _calculator.Calculate(routeName, categoryName, seatType);
        }

        // Método para crear el ticket
        public Ticket CreateTicket(string route, string category, string seat, IPriceStrategy strategy, ISession session)
        {
            // Obtener el CustomerId del usuario logueado desde la sesión
            var customerId = session.GetInt32("CustomerId");

            // Si el CustomerId es null, no se puede proceder
            if (customerId == null)
            {
                throw new Exception("No se encontró al cliente en la sesión.");
            }

            // Recuperamos el cliente existente de la base de datos
            var customer = _context.Client.FirstOrDefault(c => c.ClientId == customerId.Value);
            if (customer == null)
            {
                throw new Exception("Cliente no encontrado en la base de datos.");
            }

            // Calculamos el precio
            var price = _calculator.Calculate(route, category, seat);

            // Creamos el ticket usando el factory
            var ticket = _factory.CreateTicket(route, category, seat, price, customer);

            try
            {
                // Guardamos el ticket en la base de datos
                _context.Tickets.Add(ticket);
                _context.SaveChanges();  // Guardamos los cambios en la base de datos
            }
            catch (Exception ex)
            {
                // Manejo de errores si ocurre un problema al guardar el ticket
                throw new Exception($"Error al guardar el ticket en la base de datos: {ex.Message}", ex);
            }

            // Notificamos a todos los observadores (ej. CustomerNotifier)
            NotifyAll(ticket);

            return ticket;
        }
    }
}
