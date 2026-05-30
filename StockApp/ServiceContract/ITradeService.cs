using ServiceContract.DTOs;

namespace ServiceContract
{
    public interface ITradeService
    {
        public Task<ResponseBuyOrder> AddBuyOrder(AddRequestBuyOrder addRequestBuyOrder);

        public Task<List<ResponseBuyOrder>> GetBuyOrders();

        public Task<ResponseSellOrder> AddSellOrder(AddRequestSellOrder addRequestSellOrder);
        public Task<List<ResponseSellOrder>> GetSellOrders();
    }
}
