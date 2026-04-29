using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class AddRequestBuyOrder
    {
        public string? companyName { get; set; }
        public int shares { get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }

        public BuyOrder toBuyOrder()
        {
            BuyOrder temp = new BuyOrder()
            {
                Id = Guid.NewGuid(),
                companyName = companyName,
                shares = shares,
                price = price,
                date = date
            };
            return temp;
        }
    }
}
