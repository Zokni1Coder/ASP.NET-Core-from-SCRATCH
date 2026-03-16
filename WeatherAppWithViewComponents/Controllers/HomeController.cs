using Microsoft.AspNetCore.Mvc;
using WeatherAppWithViewComponents.Models;

namespace WeatherAppWithViewComponents.Controllers
{
    public class HomeController : Controller
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
        [Route("/")]
        public IActionResult Index()
        {
            return View(cityList);
        }

        [Route("/weather/{cityCode}")]
        public IActionResult Select(string cityCode)
        {
            City? temp = cityList.Where(code => code.CityUniqueCode == cityCode.ToUpper()).FirstOrDefault();

            return View(temp);
        }
    }
}
