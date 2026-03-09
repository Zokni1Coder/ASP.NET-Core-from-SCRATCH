using Microsoft.AspNetCore.Mvc;
using Weather_App.Models;

namespace Weather_App.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            List<City> cityList = new List<City>()
            {
            new City()
            {
                CityUniqueCode = "LND",
                CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
                TemperatureFahrenheit = 33
            },
            new City()
            {
                CityUniqueCode = "NY",
                CityName = "New York",
                DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),
                TemperatureFahrenheit = 60
            },
            new City()
            {
                CityUniqueCode = "PAR",
                CityName = "Paris",
                DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),
                TemperatureFahrenheit = 82
            }
    };
            return View(cityList);
        }
    }
}
