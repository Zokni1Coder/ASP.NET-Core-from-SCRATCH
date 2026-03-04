using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/home")]
        [Route("/")]
        public IActionResult Index()
        {
            //Hogyan kell a mappa-rendszert létrehozni?
            //0.) Hozd létre a Views mappát. Ebbe fog az össze View kerülni.
            //1.) Controllerekre bontva, hozz létre mappákat. Esetünkben ez a /View/Home mappa.
            //2.) Minden View, ami az adott Controllerhez tartozik, az annak megfelelő almappába mentsd. Esetünkben minden View, amit a Home Controller meg fog jeleníteni valamikor, a Views/Home mappába kerül.


            //Mint ahogy láthatod, minden definiálási rész átkerült ide.
            ViewData["title"] = "Home - From c# block";
            Person person = new Person()
            {
                DateOfBirth = Convert.ToDateTime("2005-05-18"),
                Name = "Réka",
                Gender = Gender.female,
                FavoriteCities = new List<string> { "Szabadka", "Salzburg", "Eger", "Rimini" }
            };
            ViewData["person"] = person;

            ViewData["alertMessage"] = $"<script>alert('{person.Name}-nak/nek {person.FavoriteCities.Count} kedvenc városa van!')</script>";

            return View(); //Ha nem adunk meg paraméterül View-t, akkor mindig ezt a sablont követi: "/View/Controller-név/Controller-metódusnév.cshtml". Tehát esetünkben nem kell most semmit sem kihangsúlyozni, mert a View útvonala és neve: "/Views/Home/Index.cshtml"

            //return View(ABC); => /View/Home/ABC.cshtml
        }
    }
}
