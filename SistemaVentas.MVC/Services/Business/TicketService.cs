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

        public TicketService(IBoletoFactory factory, PriceCalculator calculator)
        {
            _factory = factory;
            _calculator = calculator;
        }

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        private void NotifyAll(Ticket ticket)
        {
            foreach (var observer in _observers)
            {
                observer.Update(ticket);
            }
        }

        public Ticket CreateTicket(string route, string category, string seat, IPriceStrategy strategy, Client customer)
        {
            // Configuramos la estrategia de precio
            _calculator.SetStrategy(strategy);

            // Calculamos el precio usando la estrategia seleccionada
            var price = _calculator.Calculate(route, category, seat);

            // Creamos el ticket usando el factory
            var ticket = _factory.CreateTicket(route, category, seat, price, customer);

            // Aquí guardamos el ticket en la base de datos (esto es opcional, si quieres persistirlo de inmediato)
            // _context.Tickets.Add(ticket);
            // _context.SaveChanges();

            // Notificamos a todos los observadores (ej. CustomerNotifier)
            NotifyAll(ticket);

            return ticket;
        }
    }
}
