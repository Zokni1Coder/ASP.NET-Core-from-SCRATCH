using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class AddRequestSellOrder
    {
        public string? companyName { get; set; }
        public int shares { get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }

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
