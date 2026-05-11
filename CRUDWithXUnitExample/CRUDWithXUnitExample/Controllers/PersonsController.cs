using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContract;
using ServiceContract.DTOs;
using ServiceContract.Enums;

namespace CRUDWithXUnitExample.Controllers
{
    //[Route("persons")] //prefix-ként funkcionál.
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly IPersonService _personService;
        public PersonsController(ICountryService countryService, IPersonService personService)
        {
            this._countryService = countryService;
            this._personService = personService;
        }
        //[Route("index")] //url: "persons/index"
        [HttpGet("[action]")]
        [Route("/")]   //url: "persons"
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

        //[Route("create")] //url: "persons/create"
        [HttpGet("[action]")]
        public IActionResult Create()
        {
            List<CountryResponse> countries_to_View = _countryService.GetAllCountries();
            ViewBag.Countries = countries_to_View;

            //Ezzel a Linq lépéssel minden Country egyed egy SelectListItem lesz a megadott attribútumokkal.
            //Az eredmény:
            //<option value="country.CountryID">country.Name</option>
            ViewBag.Countries = countries_to_View.Select(country => new SelectListItem() { Text = country.Name, Value = country.CountryID.ToString() });

            //Egyszerű példa magyarázat helyett:
            //SelectListItem items = new SelectListItem()
            //{
            //    Text = "Reka",
            //    Value = "1"
            //};
            //Ez megjelenítve cshtml-ben:
            //<option value="1">Reka</option>

            ViewBag.Genders = Enum.GetNames(typeof(Gender));
            //Itt nem kell megjelölni a strongly type modellt, mivel nem akarunk neki semmilyen adatát megjeleníteni.
            return View();
        }

        [HttpPost("[action]")]
        //[Route("create")]
        //Efféle model binding esetén fontos hogy a html-ben szereplő elem "name" azonos legyen az objektum property nevével. Pl: <input name="PersonName"/> DTO prop: string PersonName.
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            List<CountryResponse> countries_to_View = this._countryService.GetAllCountries();

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

            PersonResponse personRespons = this._personService.AddPerson(personAddRequest);

            return RedirectToAction("Index", "persons");
        }

        [HttpGet("[action]")]
        public IActionResult Delete(PersonUpdateRequest person)
        {
            if (person is null)
            {
                return RedirectToAction("index", "persons");
            }

            return View(person);
        }

        [HttpPost("[action]")]
        public IActionResult Delete(Guid personId)
        {
            bool isSuccess = this._personService.DeletePerson(personId);
            return RedirectToAction("index", "persons");
        }

        [HttpGet("[action]")]
        public IActionResult Update(Guid personId)
        {
            List<CountryResponse> countries_to_View = _countryService.GetAllCountries();
            ViewBag.Countries = countries_to_View;

            ViewBag.Countries = countries_to_View.Select(country => new SelectListItem() { Text = country.Name, Value = country.CountryID.ToString() });

            ViewBag.Genders = Enum.GetNames(typeof(Gender));

            PersonUpdateRequest personUpdateRequest = this._personService.GetPersonById(personId).ToPersonUpdateRequest();            

            return View(personUpdateRequest);
        }

        [HttpPost("[action]")]
        public IActionResult Update(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse personResponse = this._personService.UpdatePerson(personUpdateRequest);

            if (personResponse is null)
            {
                List<CountryResponse> countries_to_View = _countryService.GetAllCountries();
                ViewBag.Countries = countries_to_View;

                ViewBag.Countries = countries_to_View.Select(country => new SelectListItem() { Text = country.Name, Value = country.CountryID.ToString() });

                ViewBag.Genders = Enum.GetNames(typeof(Gender));
                return View();
            }

            return RedirectToAction("index", "persons");
        }
    }
}
