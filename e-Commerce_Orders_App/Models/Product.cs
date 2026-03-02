using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace e_Commerce_Orders_App.Models
{
    public class Product
    {
        [Required(ErrorMessage = "The {0} can't be blank.")]
        [DisplayName("Product code")]
        public int? ProductCode { get; set; }
        [Required(ErrorMessage = "The {0} can't be blank.")]
        public int? Price { get; set; }
        [Required(ErrorMessage = "The {0} can't be blank.")]
        public int? Quantity { get; set; }
    }
}
