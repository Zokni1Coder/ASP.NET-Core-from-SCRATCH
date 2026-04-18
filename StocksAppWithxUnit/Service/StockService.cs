using ServiceContract;
using ServiceContract.DTOs;
using ServiceContract.Helpers;
using StocksAppWithxUnit.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? request)
        {
            if (request == null) throw new ArgumentNullException();
            ValidatorHelper.StockServiceValidation(request);

            BuyOrder temp = request.ToBuyOrder();
            this._buyOrders.Add(temp);

            return temp.ToBuyOrderResponse();
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest? request)
        {
            if (request == null) throw new ArgumentNullException();
            ValidatorHelper.StockServiceValidation(request);

            SellOrder sellOrder = request.ToSellOrder();
            this._sellOrders.Add(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            return this._buyOrders.Select(order => order.ToBuyOrderResponse()).ToList();
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            return this._sellOrders.Select(order => order.ToSellOrderResponse()).ToList();
        }
    }
}
