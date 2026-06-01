using Domain.CustomValidators;
using StocksAppWithxUnit.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class SellOrderRequest
    {
        [Required (ErrorMessage = "Stock symbol can't be blank.")]
        public string? StockSymbol { get; set; }
        [Required (ErrorMessage = "Stock name can't be blank.")]
        public string? StockName { get; set; }
        [MinimumYearValidator]
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "Quantity should be betweeen 1 and 100000.")]
        public uint Quantity { get; set; }
        [Range(1, 10000, ErrorMessage = "Price should be betweeen 1 and 10000.")]
        public double Price { get; set; }
        /// <summary>
        /// Az aktuális objektumot SellOrder-ré alakítjuk és generálunk egy új Id-t.
        /// </summary>
        /// <returns>SellOrder objektumot ad vissza</returns>
        public SellOrder ToSellOrder()
        {
            return new SellOrder()
            {
                SellOrderID = Guid.NewGuid(),
                StockSymbol = this.StockSymbol,
                StockName = this.StockName,
                Price = this.Price,
                Quantity = this.Quantity,
                DateAndTimeOfOrder = this.DateAndTimeOfOrder
            };
        }
    }
}
