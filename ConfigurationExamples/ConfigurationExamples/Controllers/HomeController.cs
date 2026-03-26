using Microsoft.AspNetCore.Mvc;

namespace ConfigurationExamples.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [Route("/")]
        public IActionResult Index()
        {
            IConfigurationSection section = _configuration.GetSection("MasterKey");

            ViewBag.Configuration = this._configuration["MyKey"];
            ViewBag.ClientID = section["ClientID"];
            ViewBag.ClientSecret = section["ClientSecret"];
            return View();
        }
    }
}
