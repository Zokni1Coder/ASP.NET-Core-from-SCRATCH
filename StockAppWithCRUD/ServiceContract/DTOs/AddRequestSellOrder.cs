using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class AddRequestSellOrder
    {
        public string? companyName { get; set; }
        [Range(1, 100000, ErrorMessage = "A quantity (share) should be between 1 and 100000")]
        public int shares { get; set; }
        [Range(1, 10000, ErrorMessage = "A price should be between 1 and 10000")]
        public double price { get; set; } = 0;
        public DateTime date { get; set; } = DateTime.Now;

        public SellOrder toSellOrder()
        {
            return new SellOrder()
            {
                companyName = companyName,
                shares = shares,
                price = price,
                date = date
            };
        }
    }
}
