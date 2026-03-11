using Microsoft.AspNetCore.Mvc;

namespace LayoutViewsExample.Controllers
{
    public class ProductController : Controller
    {
        [Route("products")]
        public IActionResult Product()
        {
            return View();
        }

        [Route("search/{productID?}")]
        public IActionResult Search(int? productID)
        {            
            ViewBag.ProductID = productID;  
            return View();
        }
        [Route("order")]
        public IActionResult Order()
        {
            return View();
        }
    }
}
