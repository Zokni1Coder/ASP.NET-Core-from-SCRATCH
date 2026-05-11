using ServiceContract.DTOs;

namespace ServiceContract
{
    public interface ITradeService
    {
        public ResponseBuyOrder AddBuyOrder(AddRequestBuyOrder addRequestBuyOrder);

        public List<ResponseBuyOrder> GetBuyOrders();

        public ResponseSellOrder AddSellOrder(AddRequestSellOrder addRequestSellOrder);
        public List<ResponseSellOrder> GetSellOrders();
    }
}
