using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BuyOrder
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string? stockName { get; set; }
        [MaxLength(10)]
        [Required]
        public string? stockSymbol { get; set; }
        [Required]
        public int shares { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public DateTime date { get; set; }
    }
}
