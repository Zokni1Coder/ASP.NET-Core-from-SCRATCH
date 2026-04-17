using ServiceContract.DTOs;

namespace ServiceContract
{
    public interface IStocksService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request);
        /// <summary>
        /// Lekérjük az összes BuyOrder objektumot.
        /// </summary>
        /// <returns>BuyOrderResponse típusú listát ad vissza.</returns>
        public Task<List<BuyOrderResponse>> GetBuyOrders();
        /// <summary>
        /// Lekérjük az összes SellOrder objektumot.
        /// </summary>
        /// <returns>BuyOrderResponse típusú listát ad vissza.</returns>
        public Task<List<SellOrderResponse>> GetSellOrders();
    }
}
