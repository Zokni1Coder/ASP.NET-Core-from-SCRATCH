using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationExample.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModelValidationExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/register")]
        public IActionResult Index(Person person)
        {
            if (!ModelState.IsValid)
            {
                //List<string> errorList = new List<string>(); 
                //foreach (var value in ModelState.Values)
                //{
                //    foreach (var error in value.Errors)
                //    {
                //        errorList.Add(error.ErrorMessage);
                //    }
                //}
                ////A string.Join átalakít bármilyen tömböt/listát egy string-gé.
                //string errorMessage = string.Join("\n", errorList);

                //A fenti hosszú kóddal megegyező a következő LINQ kód:
                string errorMessage = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage));
                return Content(errorMessage);

                //Magyarázat: A ModelState-et "laposítjuk" (SelectMany), azaz minden értéke egyetlen "lapos" listába kerül, mivel:

                //A ModelState felépítése egy egyfajta Dictionary (mezőnév - ModelStateEntry key-value párok).

                //Mivel minden entry több hibát is tartalmazhat, a SelectMany segítségével az összes hibát egy lapos kollekcióba gyűjtjük a SelectMany résszel.

                //Select segítségével kinyerjük az ErrorMessage property-t.

                //ModelState felépítése:
                //ModelStateDictionary
                //  ├── "Email" → ModelStateEntry
                //  │               ├── Errors[Error1, Error2]
                //  ├── "Password" → ModelStateEntry
                //                  ├── Errors[Error3]

                //SelectMany után:
                // [Error1, Error2, Error3]
            }
            return Content($"{person}");
        }
    }
}
