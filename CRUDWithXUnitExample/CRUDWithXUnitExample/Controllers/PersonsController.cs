using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using ServiceContract.DTOs;

namespace CRUDWithXUnitExample.Controllers
{
    public class PersonsController : Controller
    {
        private readonly ICountryService _countryService;

        public PersonsController(ICountryService countryService)
        {
            this._countryService = countryService;
        }
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index()
        {
            //CountryResponse countryResponse = this._countryService.AddCountry();
            return View();
        }
    }
}
