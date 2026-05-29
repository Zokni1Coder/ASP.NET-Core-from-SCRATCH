using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SellOrder
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string? stockName { get; set; }
        [MaxLength(10)]
        [Required]
        public string? StockSymbol { get; set; }
        [Required]
        public int shares { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public DateTime date { get; set; }
    }
}
