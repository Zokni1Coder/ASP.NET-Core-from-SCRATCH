using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SellOrder
    {
        public Guid Id { get; set; }
        public string? companyName { get; set; }
        public int shares { get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }
    }
}
