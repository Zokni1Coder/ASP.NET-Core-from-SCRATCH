using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Controllers
{
    public class HelloController
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

        //[Route("sayhello")] //Ezzel adjuk meg az URL-ben az elérési útvonalát. Ezt hívjuk Routing attributumnak.
        //[Route("/")]
        //[Route("sayhello2")]//Megadhatunk több útvonalat is erre a függvényre.
        //public string Hello()
        //{
        //    return "Hello from HelloController!";
        //}
    }
}
