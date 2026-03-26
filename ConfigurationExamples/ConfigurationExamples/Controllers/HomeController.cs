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
            ApiOptions? options = _configuration.GetSection("MasterKey").Get<ApiOptions>();
            //IConfigurationSection section = _configuration.GetSection("MasterKey");

            //ViewBag.Configuration = this._configuration["MyKey"];
            ViewBag.ClientID = options.ClientID;
            ViewBag.ClientSecret = options.ClientSecret;
            return View();
        }
    }
}
