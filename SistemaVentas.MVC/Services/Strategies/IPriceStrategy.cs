namespace SistemaVentas.MVC.Services.Strategies
{
    // Interface para la estrategia de cálculo de precio
    public interface IPriceStrategy
    {
        double Calculate(string routeName, string categoryName, string seatType);
    }
}
