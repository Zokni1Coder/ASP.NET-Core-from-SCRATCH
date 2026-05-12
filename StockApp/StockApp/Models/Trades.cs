using ServiceContract.DTOs;

namespace StockApp.Models
{
    public class Trades
    {
        public List<ResponseBuyOrder>? buyOrders { get; set; }
        public List<ResponseSellOrder>? sellOrders { get; set; }
    }
}
