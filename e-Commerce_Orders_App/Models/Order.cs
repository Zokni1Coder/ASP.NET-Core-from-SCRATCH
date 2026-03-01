using System.ComponentModel.DataAnnotations;

namespace e_Commerce_Orders_App.Models
{
    public class Order : IValidatableObject
    {
        private static readonly Random rnd = new Random();
        int OrderNo = rnd.Next(1, 100000);
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public List<Product> Products { get; set; }
        [Required]
        public double InvoicePrice { get; set; }

        public double ProductsSumPrice()
        {
            double sumPrice = 0;
            foreach (var product in Products)
            {
                sumPrice += (product.Price) * (product.Quantity);
            }
            return sumPrice;
        }

        public override string ToString()
        {
            return $"Order Object - OrderNo: {this.OrderNo}, Date: {this.OrderDate}, Products: ({string.Join(", ", Products.Select(No => No.ProductCode))}), Price: {ProductsSumPrice()}$.";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
