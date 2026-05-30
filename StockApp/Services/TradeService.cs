using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContract;
using ServiceContract.DTOs;
using ServiceContract.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TradeService : ITradeService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;

        //private readonly List<BuyOrder> _buyOrders;
        //private readonly List<SellOrder> _sellOrders;
        public TradeService(StockMarketDbContext stockMarketDbContext)
        {
            this._stockMarketDbContext = stockMarketDbContext;
            //this._buyOrders = new List<BuyOrder>();
            //this._sellOrders = new List<SellOrder>();
        }
        public async Task<ResponseBuyOrder> AddBuyOrder(AddRequestBuyOrder addRequestBuyOrder)
        {
            ValidationHelper.AddRequestValidator(addRequestBuyOrder);
            BuyOrder newBuyOrder = addRequestBuyOrder.toBuyOrder();
            //this._buyOrders.Add(newBuyOrder);
            int affectedRows = await
                this._stockMarketDbContext.InsertBuyOrder(newBuyOrder);
            await this._stockMarketDbContext.SaveChangesAsync();
            return newBuyOrder.toBuyOrderResponse();
        }

        public async Task<ResponseSellOrder> AddSellOrder(AddRequestSellOrder addRequestSellOrder)
        {
            ValidationHelper.AddRequestValidator(addRequestSellOrder);
            SellOrder newSellOrder = addRequestSellOrder.toSellOrder();
            //this._sellOrders.Add(newSellOrder);
            int affectedRows = await this._stockMarketDbContext.InsertSellOrder(newSellOrder);
            await this._stockMarketDbContext.SaveChangesAsync();
            return newSellOrder.toResponseSellOrder();
        }

        public async Task<List<ResponseBuyOrder>> GetBuyOrders()
        {
            //return this._buyOrders.Select(order => order.toBuyOrderResponse()).ToList();
            List<ResponseBuyOrder> responseBuyOrders = await this._stockMarketDbContext.BuyOrders.Select(order => order.toBuyOrderResponse()).ToListAsync();
            return responseBuyOrders;
        }

        public async Task<List<ResponseSellOrder>> GetSellOrders()
        {
            //return this._sellOrders.Select(order => order.toResponseSellOrder()).ToList();
            List<ResponseSellOrder> responseSellOrders = await this._stockMarketDbContext.SellOrders.Select(order => order.toResponseSellOrder()).ToListAsync();
            return responseSellOrders;
        }
    }
}
