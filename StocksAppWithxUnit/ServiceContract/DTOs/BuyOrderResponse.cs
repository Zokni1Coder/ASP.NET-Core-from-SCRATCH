using StocksAppWithxUnit.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderId { get; set; }
        [Required]
        public string? StockSymbol { get; set; }
        [Required]
        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint Quantity { get; set; }
        [Range(1, 10000)]
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        /// <summary>
        /// Két BuyOrderResponse akkor azonos ha mind a kettőnek az Id-je azonos.
        /// </summary>
        /// <param name="obj">Az aktuális objektumot a paramétereül kapottal fogja összehasonlítani.</param>
        /// <returns>Igaz vagy hamis értéket ad vissza.</returns>
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is BuyOrderResponse)
            {
                BuyOrderResponse? temp = obj as BuyOrderResponse;
                return temp?.BuyOrderId == this.BuyOrderId; 
            }
            return false;
        }
        /// <summary>
        /// Kiíratjuk az objektum összes paraméterét.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"BuyOrderId: {this.BuyOrderId}, StockSymbol: {this.StockSymbol}, StockName: {this.StockName}, DateOfOrder: {this.DateAndTimeOfOrder}, Quantity {this.Quantity}, Price: {this.Price}, , TradeAmount: {this.TradeAmount}";
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    public static class BuyOrderExtension
    {
        /// <summary>
        /// Átalakítja az aktuális BuyOrder objektumot egy BuyOrderResponse-á.
        /// </summary>
        /// <param name="buyOrder">Az aktuális átalakítandó objektum</param>
        /// <returns>Visszaadja az átalakított objektumot</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this 
            BuyOrder buyOrder)
        {
            BuyOrderResponse temp = new BuyOrderResponse()
            {
                 BuyOrderId = buyOrder.OrderId,
                 StockSymbol = buyOrder.StockSymbol,
                 Price = buyOrder.Price,
                 Quantity = buyOrder.Quantity,
                 StockName = buyOrder.StockName,
                 DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder
            };

            return temp;
        }
    }
}
