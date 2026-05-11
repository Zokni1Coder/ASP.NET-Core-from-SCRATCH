using Entities;
using ServiceContract.Helper.CustomValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class AddRequestBuyOrder
    {
        [Required(ErrorMessage = "The Stock Name can't be blank.")]
        public string? stockName { get; set; }
        [Required(ErrorMessage = "The Stock Symbol can't be blank.")]
        public string? stockSymbol { get; set; }
        [Range(1, 100000, ErrorMessage = "A quantity (share) should be between 1 and 100000")]
        public int shares { get; set; } = 0;
        [Range(1, 10000, ErrorMessage = "A price should be between 1 and 10000")]
        public double price { get; set; } = 0;
        [DateValidation]
        public DateTime date { get; set; } = DateTime.Now;

        public BuyOrder toBuyOrder()
        {
            BuyOrder temp = new BuyOrder()
            {
                Id = Guid.NewGuid(),
                stockName = stockName,
                stockSymbol = stockSymbol,
                shares = shares,
                price = price,
                date = date
            };
            return temp;
        }
    }
}
