using e_Commerce_Orders_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce_Orders_App.Controllers
{
    public class HomeController : Controller
    {
        [Route("/order")]
        public IActionResult Index(Order order)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = ModelState.Values.SelectMany(values => values.Errors).Select(err => err.ErrorMessage).ToString();
                return BadRequest(errorMessage);
            }
            return Content(order.ToString());
        }
    }
}
