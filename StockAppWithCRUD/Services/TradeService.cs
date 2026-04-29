using Entities;
using ServiceContract;
using ServiceContract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TradeService : ITradeService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;
        public TradeService()
        {
            this._buyOrders = new List<BuyOrder>();
            this._sellOrders = new List<SellOrder>();
        }
        public ResponseBuyOrder AddBuyOrder(AddRequestBuyOrder addRequestBuyOrder)
        {
            BuyOrder newBuyOrder = addRequestBuyOrder.toBuyOrder();
            this._buyOrders.Add(newBuyOrder);
            return newBuyOrder.toBuyOrderResponse();
        }

        public ResponseSellOrder AddSellOrder(AddRequestSellOrder addRequestSellOrder)
        {
            throw new NotImplementedException();
        }

        public List<ResponseBuyOrder> GetBuyOrders()
        {
            return this._buyOrders.Select(order => order.toBuyOrderResponse()).ToList();
        }

        public List<ResponseSellOrder> GetSellOrders()
        {
            return this._sellOrders.Select(order => order.toResponseSellOrder()).ToList();
        }
    }
}
