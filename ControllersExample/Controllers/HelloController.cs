using ControllersExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Controllers
{
    public class HelloController : Controller
    {
        [Route("/")]
        [Route("home")]
        public string Home()
        {
            return "Welcome from Home method.";
        }

        [Route("about")]
        public string About()
        {
            return "Welcome from About method.";
        }

        //Mint ahogy láthatod, itt is meg tudunk adni paramétert és megszorítást. Akár sajátot is.
        [Route("contact-us/{phoneNumber:regex(^\\d{{10}}$)}")]//Ez egy regex, ami elfogadja a 10db szám paramétert. Pl: 0123456789, de a 012345678 nem lesz már jó a hossza miatt.
        public string ContactUs()
        {
            return "Welcome from ContactUs method.";
        }

        // A ContentResult objektumot adjuk vissza, amit az ASP.NET Core HTTP válasszá alakít.
        [Route("cr")]
        public ContentResult CR()
        {
            return new ContentResult()
            {
                Content = "Hello from ContentResult!",//response body-ba kerül
                ContentType = "text/html",//response header-be kerül
                StatusCode = 200 //response header-be kerül
            };
        }

        //Ez ugyanaz, mint a CR megoldás ContentResult-tal, csak rövidebb. Ha így akarod használni, okvetlen meg kell jelölni a Class-nak a Controller-t, mint szülő!
        [Route("ct")]
        public ContentResult CT()
        {
            return Content("Hello from Content!", "text/html");
        }

        [Route("personv1")]
        public JsonResult PersonV1()
        {
            Person person = new Person { ID = Guid.NewGuid(), firstName = "Reka", lastName = "Granyak", age = 20 };
            return new JsonResult(person);
        }

        [Route("personv2")]
        public JsonResult PersonV2()
        {
            Person person = new Person { ID = Guid.NewGuid(), firstName = "Reka", lastName = "Granyak", age = 20 };
            return Json(person); //itt különböznek. Ezt a változatot használják leggyakrabban, mert rövidebb és "clean".
        }


        //[Route("sayhello")] //Ezzel adjuk meg az URL-ben az elérési útvonalát. Ezt hívjuk Routing attributumnak.
        //[Route("/")]
        //[Route("sayhello2")]//Megadhatunk több útvonalat is erre a függvényre.
        //public string Hello()
        //{
        //    return "Hello from HelloController!";
        //}
    }
}
