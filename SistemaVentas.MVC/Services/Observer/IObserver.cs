using SistemaVentas.Model;

namespace SistemaVentas.MVC.Services.Observers
{
    /// <summary>
    /// Observador que reacciona ante la creación de un ticket.
    /// </summary>
    public interface IObserver
    {
        void Update(Ticket ticket);
    }
}