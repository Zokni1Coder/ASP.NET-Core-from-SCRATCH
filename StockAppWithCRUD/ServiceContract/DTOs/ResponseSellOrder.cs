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
        public string? date { get; set; }
        public double tradeAmount { get; set; }

        public override string ToString()
        {
            return $"RBuyOrder: id: {this.id}, companyName: {this.companyName}, shares: {this.shares}, price: {this.price}, date: {this.date}, tradeAmount: {this.tradeAmount}";
        }

        public override bool Equals(object? obj)
        {
            if (obj != null && obj is ResponseSellOrder)
            {
                ResponseSellOrder temp = (ResponseSellOrder)obj;
                return (this.id == temp.id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public static class SellOrderExtension
    {
       public static ResponseSellOrder toResponseSellOrder(this SellOrder sellOrder)
        {
            return new ResponseSellOrder()
            {
                shares = sellOrder.shares,
                price = Math.Round(sellOrder.price, 2),
                date = sellOrder.date.ToString("MM/dd/yyyy hh:mm tt"),
                companyName = sellOrder.companyName,
                id = sellOrder.Id,
                tradeAmount = Math.Round(sellOrder.price * sellOrder.shares,2 )
            };
        }
    }
}
