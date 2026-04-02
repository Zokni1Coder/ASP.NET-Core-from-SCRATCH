namespace StockApp.Models
{
    public class Stock
    {
        public string? StockSymbol { get; set; }
        public double CurrentPrice { get; set; }
        public double HighPriceOfDay { get; set; }
        public double LowPriceOfDay { get; set; }
        public double OpenPrice { get; set; }

    }
}
