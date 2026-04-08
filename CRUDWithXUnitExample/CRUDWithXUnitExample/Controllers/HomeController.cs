using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using ServiceContract.DTOs;

namespace CRUDWithXUnitExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICountryService _countryService;

        public HomeController(ICountryService countryService)
        {
            this._countryService = countryService;
        }
        public IActionResult Index()
        {
            //CountryResponse countryResponse = this._countryService.AddCountry();
            return View();
        }
    }
}
