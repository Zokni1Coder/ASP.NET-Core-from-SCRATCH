using System.ComponentModel.DataAnnotations;

namespace e_Commerce_Orders_App.Models
{
    public class Product
    {
        [Required]
        public int ProductCode { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
