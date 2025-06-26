namespace SistemaVentas.MVC.Services.Strategies
{
    /// <summary>
    /// Orquesta el cálculo de precio usando una estrategia inyectada.
    /// </summary>
    public class PriceCalculator
    {
        private IPriceStrategy _strategy = null!;

        /// <summary>
        /// Configura la estrategia de cálculo.
        /// </summary>
        public void SetStrategy(IPriceStrategy strategy)
            => _strategy = strategy;

        /// <summary>
        /// Calcula el precio aplicando la estrategia configurada.
        /// </summary>
        public double Calculate(string routeName, string categoryName, string seatType)
            => _strategy.Calculate(routeName, categoryName, seatType);
    }
}
