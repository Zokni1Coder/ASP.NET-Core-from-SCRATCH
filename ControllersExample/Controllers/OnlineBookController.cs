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
                Response.StatusCode = 401; //Ez a autentikáció hiányának kódja.
                return Content("You must be logged in if you want to reach the book!");
            }

            bool isLoggedIn = Convert.ToBoolean(ControllerContext.HttpContext.Request.Query["isloggedin"]);

            if (isLoggedIn == false)
            {
                Response.StatusCode = 401;
                return Content("You must be logged in if you want to reach the book!");
            }
            //Ha a bookid paraméter nincs megadva az url-ben.
            if (!Request.Query.ContainsKey("bookid"))
            {
                Response.StatusCode = 400;
                return Content("The Book id is not supplied!");
            }
            //Ha a bookid kissebb mint 1 vagy nagyobb mint 1000 vagy null az értéke.
            int bookId = Convert.ToInt32(Request.Query["bookid"]);
            if (bookId <= 0 || bookId > 1000)
            {
                Response.StatusCode = 400;
                return Content("The Book id must be between 1 and 1000!");
            }
            else
            {
                return File("/DPMunka.pdf", "application/pdf");
            }

        }
    }
}
