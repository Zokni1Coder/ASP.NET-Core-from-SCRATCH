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
                string errorMessage = string.Join("\n", ModelState.Values.SelectMany(values => values.Errors).Select(err => err.ErrorMessage));
                return BadRequest(errorMessage);
            }
            Random rnd = new Random();
            order.OrderNo = rnd.Next(1, 100000);
            return new JsonResult(order.ToString());
        }
    }
}
