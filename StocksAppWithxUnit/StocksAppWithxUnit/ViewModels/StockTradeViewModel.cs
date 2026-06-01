namespace StocksAppWithxUnit.ViewModels
{
    /// <summary>
    /// Ezt fogjukfelhasználni az Index viewban az adatok megjelenítésére.
    /// </summary>
    public class StockTradeViewModel
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public double Price { get; set; }
        public uint? Quantity { get; set; }
    }
}
