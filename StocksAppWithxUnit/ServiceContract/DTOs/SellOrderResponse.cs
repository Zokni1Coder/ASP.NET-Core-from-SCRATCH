using StocksAppWithxUnit.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class SellOrderResponse
    {
        public Guid SellOrderId { get; set; }
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
        /// Két SellOrderResponse akkor azonos ha mind a kettőnek az Id-je azonos.
        /// </summary>
        /// <param name="obj">Az aktuális objektumot a paramétereül kapottal fogja összehasonlítani.</param>
        /// <returns>Igaz vagy hamis értéket ad vissza.</returns>
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is SellOrderResponse)
            {
                SellOrderResponse? temp = obj as SellOrderResponse;
                return temp?.SellOrderId == this.SellOrderId;
            }
            return false;
        }
        /// <summary>
        /// Kiíratjuk az objektum összes paraméterét.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"BuyOrderId: {this.SellOrderId}, StockSymbol: {this.StockSymbol}, StockName: {this.StockName}, DateOfOrder: {this.DateAndTimeOfOrder}, Quantity {this.Quantity}, Price: {this.Price}, TradeAmount: {this.TradeAmount}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class SellOrderExtension
    {
        /// <summary>
        /// Átalakítja az aktuális SellOrder objektumot egy SellOrderResponse-á.
        /// </summary>
        /// <param name="sellOrder">Az aktuális átalakítandó objektum</param>
        /// <returns>Visszaadja az átalakított objektumot</returns>
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderId = sellOrder.SellOrderID,
                StockName = sellOrder.StockName,
                Price = sellOrder.Price,
                StockSymbol = sellOrder.StockSymbol,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity
            };
        }
    }
}
