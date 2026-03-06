using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/home")]
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
            //Ha Strongly Typed Views-t használunk, akkor nem kell megadni egy összekötő elemet sem, mint a ViewData vagy ViewBag.
            //ViewData["person"] = person;

            ViewData["alertMessage"] = $"<script>alert('{person.Name}-nak/nek {person.FavoriteCities.Count} kedvenc városa van!')</script>";


            //Paraméterül adjuk át a Person objektumot.
            return View(person); //Ha nem adunk meg paraméterül View-t, akkor mindig ezt a sablont követi: "/View/Controller-név/Controller-metódusnév.cshtml". Tehát esetünkben nem kell most semmit sem kihangsúlyozni, mert a View útvonala és neve: "/Views/Home/Index.cshtml"

            //return View(ABC); => /View/Home/ABC.cshtml
        }

        [Route("/")]//Az Index esetében persze el kell távolítani.
        public IActionResult Persons()
        {
            List<Person> people = new List<Person>
            {
                new Person()
                {
                    Name = "Reka",
                    DateOfBirth = Convert.ToDateTime("2005-05-18"),
                    Gender = Gender.female
                },
                new Person()
                {
                    Name = "Erik",
                    DateOfBirth = Convert.ToDateTime("2000-09-22"),
                    Gender = Gender.male
                },
                new Person()
                {
                    Name = "Niki",
                    DateOfBirth = Convert.ToDateTime("1996-09-17"),
                    Gender = Gender.female
                }
            };

            //Ha Strongly Typed Views-t használunk, akkor nem kell megadni egy összekötő elemet sem, mint a ViewData vagy ViewBag.
            //Paraméterül adjuk át a Person objektumot.
            return View(people);
        }

        [Route("/person/{name}")]
        public IActionResult SelectedPerson(string name)
        {
            //Nem egy jó megoldás, de később megtanuljuk megfelelően az ilyen eseteket kikerülni.
            List<Person> people = new List<Person>
            {
                new Person()
                {
                    Name = "Reka",
                    DateOfBirth = Convert.ToDateTime("2005-05-18"),
                    Gender = Gender.female
                },
                new Person()
                {
                    Name = "Erik",
                    DateOfBirth = Convert.ToDateTime("2000-09-22"),
                    Gender = Gender.male
                },
                new Person()
                {
                    Name = "Niki",
                    DateOfBirth = Convert.ToDateTime("1996-09-17"),
                    Gender = Gender.female
                }
            };

            //Linq kifejezés
            Person? selectedPerson = people.Where(person => person.Name == name).FirstOrDefault();
            return View(selectedPerson);
        }

        [Route("/person-and-product")]
        public IActionResult PersonAndProduct()
        {
            Person person = new Person()
            {
                Name = "Reka",
                DateOfBirth = Convert.ToDateTime("2005-05-18"),
                Gender = Gender.female
            };
            Product product = new Product()
            {
              Id = 1,
              Name = "Car"
            };
            PersonAndProductViewModel personAndProductViewModel = new PersonAndProductViewModel()
            {
              PersonData = person,
              ProductData = product
            };
            return View(personAndProductViewModel);
        }
    }
}
