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

        //VirtualFileResult csak akkor működik, ha a wwwroot mappában van elhelyezve a statikus fájl. 
        //Alapértelmezetten a wwwroot mappa tárol minden olyan statikus fájlt, amit szeretnénk online elérhetővé tenni.
        //Itt a RELATÍV útvonalat kell megadni!
        [Route("download1")]
        public VirtualFileResult DownloadV1()
        {
            return new VirtualFileResult("/DPMunka.pdf", "application/pdf");
        }
        //Ez ugyanaz. mint a DownloadV1, csak rövidebben és "clean".
        [Route("vdownload1s")]
        public VirtualFileResult DownloadV1Short()
        {
            return File("/DPMunka.pdf", "application/pdf");
        }
         //Ez akkor is működik, amikor a file nem a wwwrootban van, hanem bárhol máshol. Ezért itt az ABSZOLÚT elérési útvonal kell.
         //Gyakorlatban inkább a VirtualFileResult-ot alkalmazzák, biztonsági okok miatt.
        [Route("download2")]
        public PhysicalFileResult DownloadV2()
        {
            return new PhysicalFileResult(@"C:\Users\erikk\Downloads\DPMunka.pdf", "application/pdf");
        }

        //Ha fájlt adatbázisból (pl. PDF, kép) szeretnénk visszaadni, akkor azt byte[] formában kezeljük, és FileContentResult-et használunk.
        [Route("download3")]
        public FileContentResult Download3()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\Users\erikk\source\repos\ASP.NET-Core-from-SCRATCH\ControllersExample\wwwroot\DPMunka.pdf");
            return File(fileBytes, "application/pdf");
        }

        //Ez ugyanaz, mint a FileContentResult. Miért? Mert a FileContentResult is egy IActionResult, mert az IActionResult interface implementálva van benne, tehát a felületük közös. 
        [Route("polymorph")]
        public IActionResult Download4()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\Users\erikk\source\repos\ASP.NET-Core-from-SCRATCH\ControllersExample\wwwroot\DPMunka.pdf");
            return File(fileBytes, "application/pdf");
        }

        //[Route("download2s")]
        //public PhysicalFileResult DownloadV2Short()
        //{
        //    return File(@"C:\Users\erikk\Downloads\DPMunka.pdf", "application/pdf");
        //}


        //[Route("sayhello")] //Ezzel adjuk meg az URL-ben az elérési útvonalát. Ezt hívjuk Routing attributumnak.
        //[Route("/")]
        //[Route("sayhello2")]//Megadhatunk több útvonalat is erre a függvényre.
        //public string Hello()
        //{
        //    return "Hello from HelloController!";
        //}
    }
}
