using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class ResponseSellOrder
    {
        public Guid id { get; set; }
        public string? companyName { get; set; }
        public int shares { get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }
        public double tradeAmount { get; set; }
    }

    public static class SellOrderExtension
    {
       public static ResponseSellOrder toResponseSellOrder(this SellOrder sellOrder)
        {
            return new ResponseSellOrder()
            {
                shares = sellOrder.shares,
                price = sellOrder.price,
                date = sellOrder.date,
                companyName = sellOrder.companyName,
                id = sellOrder.Id,
                tradeAmount = sellOrder.price * sellOrder.shares
            };
        }
    }
}
