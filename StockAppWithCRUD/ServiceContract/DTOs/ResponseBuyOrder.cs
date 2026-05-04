using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class ResponseBuyOrder
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
            if (obj != null && obj is ResponseBuyOrder)
            {
                ResponseBuyOrder temp = (ResponseBuyOrder)obj;
                return (this.id == temp.id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    public static class BuyOrderExtension
    {
        public static ResponseBuyOrder toBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new ResponseBuyOrder()
            {
               date = buyOrder.date.ToString("MM/dd/yyyy hh:mm tt"),
               price = Math.Round(buyOrder.price, 2),
               id = buyOrder.Id,
               companyName = buyOrder.companyName,
               shares = buyOrder.shares,
               tradeAmount = Math.Round(buyOrder.price * buyOrder.shares, 2)
            };
        }
    }
}
