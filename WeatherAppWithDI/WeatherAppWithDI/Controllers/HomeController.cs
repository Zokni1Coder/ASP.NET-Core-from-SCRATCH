using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using System.Security.AccessControl;

namespace WeatherAppWithDI.Controllers
{    
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;
        public HomeController(IWeatherService weatherService)
        {
          this._weatherService = weatherService;  
        }
        [Route("/")]
        public IActionResult Index()
        {           
            return View(this._weatherService.GetWeatherDetails());
        }

        [Route("/weather/{cityCode}")]
        public IActionResult SelectedCity(string cityCode)
        {
            return View(this._weatherService.GetCity(cityCode));
        }
    }
}
