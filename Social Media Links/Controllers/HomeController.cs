using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Social_Media_Links.Options_Model;

namespace Social_Media_Links.Controllers
{
    public class HomeController : Controller
    {
        private readonly SocialMediaLinksOptions _socialMediaOptions;

        public HomeController(IOptions<SocialMediaLinksOptions> options)
        {
            this._socialMediaOptions = options.Value;
        }
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.Title = "Home";
            ViewBag.SocialMedias = _socialMediaOptions;
            return View();
        }
    }
}
