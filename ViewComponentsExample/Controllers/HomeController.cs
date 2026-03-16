using Microsoft.AspNetCore.Mvc;
using ViewComponentsExample.Models;

namespace ViewComponentsExample.Controllers
{
    public class HomeController : Controller
    {

        /*
            Futási sorrend szemléltetve:    

            Controller
               ↓
            View
               ├─ Partial
               ├─ ViewComponent
               ↓
            Layout

         */

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }
        [Route("load-cars")]
        public IActionResult LoadCars()
        {
            Manufacturer manufacturer = new Manufacturer()
            {
                Brand = "Audi",
                Models = new List<CarModel>()
                {
                    new CarModel()
                    {
                        Chassie = Chassis.limousine,
                        Model = "A4"
                    },
                    new CarModel()
                    {
                        Chassie = Chassis.hothatch,
                        Model = "A3"
                    },
                    new CarModel()
                    {
                        Chassie = Chassis.limousine,
                        Model = "A6"
                    }
                }
            };

            return ViewComponent("Grid", new { manufacturer });
        }
    }
}
