using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
    public class ProductController : Controller
    {
        [Route("/product/all")]
        public IActionResult AllProduct()
        {
            Product product = new Product()
            {
                Id = 1,
                Name = "Car"
            };
            return View(product);
        }
    }
}
