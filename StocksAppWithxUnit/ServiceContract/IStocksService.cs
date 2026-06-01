using ServiceContract.DTOs;

namespace ServiceContract
{
    public interface IStocksService
    {
        /// <summary>
        /// BuyOrder-okat készítünk egy mentjük el a listában.
        /// </summary>
        /// <param name="request">Az elmentendő objektum</param>
        /// <returns>BuyOrderResponse típust ad vissza.</returns>
        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? request);
        /// <summary>
        /// SellOrder-okat készítünk egy mentjük el a listában.
        /// </summary>
        /// <param name="request">Az elmentendő objektum</param>
        /// <returns>SellOrderResponse típust ad vissza.</returns>
        public SellOrderResponse CreateSellOrder(SellOrderRequest? request);
        /// <summary>
        /// Lekérjük az összes BuyOrder objektumot.
        /// </summary>
        /// <returns>BuyOrderResponse típusú listát ad vissza.</returns>
        public List<BuyOrderResponse> GetBuyOrders();
        /// <summary>
        /// Lekérjük az összes SellOrder objektumot.
        /// </summary>
        /// <returns>SellOrderResponse típusú listát ad vissza.</returns>
        public List<SellOrderResponse> GetSellOrders();
    }
}
