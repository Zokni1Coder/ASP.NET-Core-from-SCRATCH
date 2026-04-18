using System.ComponentModel.DataAnnotations;

namespace StocksAppWithxUnit.Entities
{
    public class BuyOrder
    {
        public Guid OrderId { get; set; }
        public string StockSymbol { get; set; }
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }       
        public uint Quantity { get; set; }
        public double Price { get; set; }

    }
}
