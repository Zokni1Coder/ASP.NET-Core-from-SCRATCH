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
            ViewBag.Configuration = this._configuration["MyKey"];
            return View();
        }
    }
}
