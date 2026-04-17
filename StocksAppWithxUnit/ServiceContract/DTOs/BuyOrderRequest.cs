using StocksAppWithxUnit.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class BuyOrderRequest
    {
        [Required]
        public string? StockSymbol { get; set; }
        [Required]
        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint Quantity { get; set; }
        [Range(1, 10000)]
        public double Price { get; set; }

        public BuyOrder ToBuyOrder()
        {
            BuyOrder temp = new BuyOrder()
            {
                OrderId = new Guid(),
                StockSymbol = StockSymbol,
                Quantity = Quantity,
                Price = Price,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder
            };
            return temp;
        }
    }
}
