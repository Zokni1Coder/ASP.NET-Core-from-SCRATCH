using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace ControllersExample.Controllers
{
    //A Feladat: Kezeld le a következő kérést: book?isloggedin=true&bookid=1. A bookid 1 és 1000 között kell hogy legyen, kezeld le azt is ha nincs bookid paraméter. Az isloggedin pedig true kell hogy legyen, hogy a könyvet elérd.
    public class OnlineBookController : Controller
    {
        [Route("book")]
        public IActionResult Index()
        {
            //Ha nincs isloggedin paraméter vagy az false.            
            if (!Request.Query.ContainsKey("isloggedin"))
            {
                //Response.StatusCode = 401; //Ez a autentikáció hiányának kódja.
                return Unauthorized("You must be logged in if you want to reach the book!");
            }

            bool isLoggedIn = Convert.ToBoolean(ControllerContext.HttpContext.Request.Query["isloggedin"]);

            if (isLoggedIn == false)
            {
                //Response.StatusCode = 401;
                return Unauthorized("You must be logged in if you want to reach the book!");
                //return StatusCode(401);//Ezt pedig akkor használjuk, amikor nem az említett gyakori státuszokat szeretnénk használni, hanem olyat ami még nincs "beépítve", mint az Unauthorized. Ezzel a response body üres lesz, de a státusz kód 401 és sokkal elegánsabb mint felül.
            }
            //Ha a bookid paraméter nincs megadva az url-ben.
            if (!Request.Query.ContainsKey("bookid"))
            {
                //Response.StatusCode = 400;
                //return Content("The Book id is not supplied!");
                return BadRequest("The Book id is not supplied!");   //Ebben az esetben alapból a StatusCode az 400 lesz, a válasz benne ugyanaz mint fent és sokkal szebb megoldás.
            }
            //Ha a bookid kissebb mint 1 vagy nagyobb mint 1000 vagy null az értéke.
            int bookId = Convert.ToInt32(Request.Query["bookid"]);
            if (bookId <= 0 || bookId > 1000)
            {
                //Response.StatusCode = 400;
                //return Content("The Book id must be between 1 and 1000!");
                return NotFound("The Book id must be between 1 and 1000!");
            }                    
            else
            {
                return File("/DPMunka.pdf", "application/pdf");
            }

        }
    }
}
