using ServiceContract;
using ServiceContract.DTOs;
using StocksAppWithxUnit.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Service
{
    public class StockService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;
        public StockService()
        {
            this._buyOrders = new List<BuyOrder>();
            this._sellOrders = new List<SellOrder>();
        }
        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            BuyOrder temp = request.ToBuyOrder();
            this._buyOrders.Add(temp);

            return Task.FromResult(temp.ToBuyOrderResponse());
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request)
        {
            SellOrder sellOrder = request.ToSellOrder();
            this._sellOrders.Add(sellOrder);

            return Task.FromResult(sellOrder.ToSellOrderResponse());
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return Task.FromResult(this._buyOrders.Select(order => order.ToBuyOrderResponse()).ToList());
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            return Task.FromResult(this._sellOrders.Select(order => order.ToSellOrderResponse()).ToList());
        }
    }
}
