using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using ServiceContract.DTOs;
using ServiceContract.Enums;

namespace CRUDWithXUnitExample.Controllers
{
    public class PersonsController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly IPersonService _personService;

        public PersonsController(ICountryService countryService, IPersonService personService)
        {
            this._countryService = countryService;
            this._personService = personService;
        }
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortingOptions sortingOptions = SortingOptions.ASC)
        {
            ViewBag.activeSorting = sortingOptions;
            ViewBag.activeSearchKey = searchBy;
            ViewBag.activeSearchValue = searchString;
            ViewBag.activeSortBy = sortBy;
            List<(string, string)> tableColumnHeaders = new List<(string, string)>() { (nameof(PersonResponse.PersonName), "Person Name"), (nameof(PersonResponse.Email), nameof(PersonResponse.Email)), (nameof(PersonResponse.DateOfBirth), "Date of Birth"), (nameof(PersonResponse.Age), nameof(PersonResponse.Age)), (nameof(PersonResponse.Gender), nameof(PersonResponse.Gender)), (nameof(PersonResponse.Country), nameof(PersonResponse.Country)), (nameof(PersonResponse.Address), nameof(PersonResponse.Address)), (nameof(PersonResponse.ReceiveNewsLetter), "Receive News Letter") };
            ViewBag.TableColumnHeaders = tableColumnHeaders;
            //A keresési opciók, a legördülő menühöz az UI-en.
            Dictionary<string, string> searchByOptions = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.Address), "Address" },
                { nameof(PersonResponse.Country), "Country" },
                { nameof(PersonResponse.Gender), "Gender" }
            };

            ViewBag.searchByOptions = searchByOptions;
            //Lekérjük az adatokat
            List<PersonResponse>? temp_persons = temp_persons = this._personService.GetFilteredPerson(searchBy, searchString);
            //Sorbarendezzük az adatokat.
            temp_persons = this._personService.GetSortedPersons(temp_persons, sortBy, sortingOptions);

            return View(temp_persons);
        }
    }
}
