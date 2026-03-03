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

            
            return View(); //Ha nem adunk meg paraméterül View-t, akkor mindig ezt a sablont követi: "/View/Controller-név/Controller-metódusnév.cshtml". Tehát esetünkben nem kell most semmit sem kihangsúlyozni, mert a View útvonala és neve: "/Views/Home/Index.cshtml"

            //return View(ABC); => /View/Home/ABC.cshtml
        }
    }
}
