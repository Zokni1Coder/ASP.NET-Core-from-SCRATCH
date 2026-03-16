using Microsoft.AspNetCore.Mvc;
using WeatherAppWithViewComponents.Models;

namespace WeatherAppWithViewComponents.Components
{
    public class WeatherViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(City city)
        {
            return View("cityWeather", city);
        }
    }
}
