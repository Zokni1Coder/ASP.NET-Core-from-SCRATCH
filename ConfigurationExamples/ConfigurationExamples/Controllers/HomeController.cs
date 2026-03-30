using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExamples.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IConfiguration _configuration;

        //Itt a megfelelő osztálytípusú mezőt hozunk létre.
        private readonly ApiOptions _options;


        //Az IoC konténer a megfelelő típusú IOptions objektumot fogja létrehozni és átadni nekünk.
        public HomeController(IOptions<ApiOptions> options)
        {
            //this._configuration = configuration;
            this._options = options.Value;
        }

        [Route("/")]
        public IActionResult Index()
        {
            //ApiOptions? options = _configuration.GetSection("MasterKey").Get<ApiOptions>();
            //IConfigurationSection section = _configuration.GetSection("MasterKey");

            //ViewBag.Configuration = this._configuration["MyKey"];
            //ViewBag.ClientID = options.ClientID;
            //ViewBag.ClientSecret = options.ClientSecret;


            ViewBag.ClientID = _options.ClientID;
            ViewBag.ClientSecret = _options.ClientSecret;
            return View();
        }
    }
}
