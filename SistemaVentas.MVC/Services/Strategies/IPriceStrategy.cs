namespace SistemaVentas.MVC.Services.Strategies
{
    //Strategy Pattern for calculating ticket prices based on route, category, and seat type
    public interface IPriceStrategy
    {
        double Calculate(string routeName, string categoryName, string seatType);
    }
}