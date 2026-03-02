using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace e_Commerce_Orders_App.Models
{
    public class Order : IValidatableObject
    {
        [BindNever]
        public int OrderNo { get; set; }
        [Required(ErrorMessage = "The {0} can't be blank.")]
        [DisplayName("Order Date")]
        public DateTime? OrderDate { get; set; }
        [Required(ErrorMessage = "The list of Product can't be blank.")]
        public List<Product>? Products { get; set; }
        [Required(ErrorMessage = "The {0} can't be blank.")]
        [DisplayName("Invoice price")]
        public double? InvoicePrice { get; set; }

        public int? ProductsSumPrice()
        {
            int? sumPrice = 0;
            foreach (var product in Products)
            {
                sumPrice += (product.Price * product.Quantity);
            }
            return sumPrice;
        }

        public override string ToString()
        {
            return $"Order Object - OrderNo: {this.OrderNo}, Date: {this.OrderDate}, Products: ({string.Join(", ", Products.Select(No => No.ProductCode))}), Price: {ProductsSumPrice()}$.";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.InvoicePrice != ProductsSumPrice())
            {
                yield return new ValidationResult("Invoice Price doesn't match with the total cost of the specified products in the order.");
            }
            if (this.OrderDate.Value.Year < 2000)
            {
                yield return new ValidationResult("Order date should be greater than or equal to 2000-01-01.");
            }
        }
    }
}
