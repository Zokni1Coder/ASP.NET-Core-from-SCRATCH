namespace Entities
{
    public class BuyOrder
    {
        public Guid Id { get; set; }
        public string? stockName { get; set; }
        public string? stockSymbol { get; set; }
        public int shares { get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }
    }
}
