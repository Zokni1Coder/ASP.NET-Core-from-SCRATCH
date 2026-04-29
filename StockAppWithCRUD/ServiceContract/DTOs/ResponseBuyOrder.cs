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
        public DateTime date { get; set; }
    }
    public static class BuyOrderExtension
    {
        public static ResponseBuyOrder toBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new ResponseBuyOrder()
            {
               date = buyOrder.date,
               price = buyOrder.price,
               id = buyOrder.Id,
               companyName = buyOrder.companyName,
               shares = buyOrder.shares,
            };
        }
    }
}
