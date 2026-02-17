using ControllersExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace ControllersExample.Controllers
{
    //A Feladat: Kezeld le a következő kérést: book?isloggedin=true&bookid=1. A bookid 1 és 1000 között kell hogy legyen, kezeld le azt is ha nincs bookid paraméter. Az isloggedin pedig true kell hogy legyen, hogy a könyvet elérd.
    public class OnlineBookController : Controller
    {
        //Példa a precedenciára: "localhost:xxx/book/10/true?bookid=5&isloggedin=false"
        //így adod meg a model bindinggal a route paramétert.
        //nullable bookid paraméter, amit a keretrendszer binding-olni fog.
        //Az alatta lévő if is ugyanazt csinálja. Inkább model bindingot használjunk!
        [Route("book/{bookid?}/{author?}/{isloggedin?}/")]        
        //Futtatva a: "http://localhost:5292/book/11/true?bookid=5&isloggedin=false" elemezd ki hogy miért fog lefutni.
        //QueryString: bookid = 5; isloggedin = false;
        //RouteParam: bookid = 11; isloggedin = true;

        //Ha behelyettesíted: public IActionResult Index(5, true){}

        //Beállítottuk, hogy a Book paraméterpéldány adatai, kizárólag a QueryStringből vegyen adatokat a propertykhez. 
        public IActionResult Index([FromQuery]int? bookid, [FromRoute] string? author, [FromRoute]bool? isloggedin, [FromQuery] Book book) 
        {
            if (bookid == null)
            {
                return BadRequest("You must be logged in if you want to reach the book!");
            }

            //Ha a bookid paraméter nincs megadva az url-ben.
            //if (!Request.Query.ContainsKey("bookid"))
            //{
            //    //Response.StatusCode = 400;
            //    //return Content("The Book id is not supplied!");
            //    return BadRequest("The Book id is not supplied!");   //Ebben az esetben alapból a StatusCode az 400 lesz, a válasz benne ugyanaz mint fent és sokkal szebb megoldás.
            //}

            //Ha nincs isloggedin paraméter vagy az false.
            //
            if ((bool)!isloggedin)
            {
                return Unauthorized("You must be logged in if you want to reach the book!");
            }
            //if (!Request.Query.ContainsKey("isloggedin"))
            //{
            //    //Response.StatusCode = 401; //Ez a autentikáció hiányának kódja.
            //    return Unauthorized("You must be logged in if you want to reach the book!");
            //}

            //bool isLoggedIn = Convert.ToBoolean(ControllerContext.HttpContext.Request.Query["isloggedin"]);

            //if (isloggedin == false)
            //{
            //    //Response.StatusCode = 401;
            //    return Unauthorized("You must be logged in if you want to reach the book!");
            //    //return StatusCode(401);//Ezt pedig akkor használjuk, amikor nem az említett gyakori státuszokat szeretnénk használni, hanem olyat ami még nincs "beépítve", mint az Unauthorized. Ezzel a response body üres lesz, de a státusz kód 401 és sokkal elegánsabb mint felül.
            //}

            //Ha a bookid kissebb mint 1 vagy nagyobb mint 1000 vagy null az értéke.
            //int bookId = Convert.ToInt32(Request.Query["bookid"]);
            if (bookid <= 0 || bookid > 1000)
            {
                //Response.StatusCode = 400;
                //return Content("The Book id must be between 1 and 1000!");
                return NotFound("The Book id must be between 1 and 1000!");
            }
            else
            {
                //return File("/DPMunka.pdf", "application/pdf");


                //Maradva a könyves példánknál, ha minden querystring megfelelő, akkor most nem a .pdf fájlt nyitjuk meg, hanem átirányítjuk a requestet a HelloController PersonV1 metódusába, ami kiír egy rövid szöveget.

                //Paraméterek:
                //-PersonV1: A HelloControllerben lévő metódus/eljárás neve, amire szeretnénk átirányítani a kérést és futtatni.
                //-Hello: A controller neve a "controller" szó nélkül.
                //-new {}: mivel nem adunk most át/tovább semmilyen értéket sem, ezért csak egy üres objektum osztályt adunk át. Hülyeség ebben az esetben. Később itt fontos adatokat tudunk továbbítani ezzel a parammal. 
                //return new RedirectToActionResult("PersonV1", "Hello", new { }); //302 - Found
                //Rövidítve:
                //return RedirectToAction("PersonV1", "Hello", new { });

                //return new RedirectToActionResult("PersonV1", "Hello", new { }, true); //Itt a true paraméterrel állítjuk be a 301-es állapotot. Jelentése benne van a füzetben. Ez az úgynevezett "permanent bool value". Tehát ez a 301 - Moved Permanently.

                //Itt most az Action-nel két paramétert is átadunk, mint RouteValues.
                //Rövidítve:
                //return RedirectToActionPermanent("PersonV1", "Hello", new { id = 1 });

                //Ugyanez LocalRedirectResult-tal:
                //return LocalRedirect("personv1");
                //return LocalRedirectPermanent("personv1");


                //Fontos, hogy a route-ot egy "/" jellel kezd, különben a meglévő aktív URL-hez appendolja ezt az útvonalat. Az aktív URL-ünk most a "localhost:xxx/book/10/true", mivel a routing parameter-ek előrébbre van, mint a query string. 

                //Ha nincs ott a "/" jel akkor a következő URL-re akar továbbítani: localhost:xxx/book/x/ct/x, viszont nekünk a localhost:xxx/book/ct/x kell.
                //return Redirect($"/ct/{bookid}");

                //Kiíratom a book objektumot. Azért tudom ToString-gel, mert felülírtuk az osztályon belül("override ToString()"). 
                return Content(book.ToString());
            }

        }
    }
}
