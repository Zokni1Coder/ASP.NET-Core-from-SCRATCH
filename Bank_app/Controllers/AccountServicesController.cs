using Bank_app.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bank_app.Controllers
{
    public class AccountServicesController : Controller
    {
        //hard-coded data
        AccountClass account = new AccountClass(1001, "Example name", 5000);

        //When request is received at path "/"
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Best Bank");
        }

        //When request is received at path "/account-details"
        [Route("/account-details")]
        public IActionResult Details()
        {
            //send the object as JSON
            return Json(account);
        }

        //When request is received at path "/account-statement"
        [Route("/account-statement")]
        public IActionResult Statement()
        {
            //send a pdf file (at wwwroot folder) as response
            return File("/DPMunka.pdf", "application/pdf");
        }

        [Route("/get-current-balance/{accountNumber?}")]
        public IActionResult CurrentBalance()
        {
            // Check if the 'accountNumber' parameter is provided
            if (!Request.RouteValues.ContainsKey("accountNumber"))
            {
                return NotFound("Account Number should be supplied");
            }
            // Convert the 'accountNumber' to an integer
            int? accountNumber = Convert.ToInt32(Request.RouteValues["accountNumber"]);
            // If the 'accountNumber' provided in the route parameter is not a valid integer, return HTTP 400
            if (accountNumber is null)
            {
                return BadRequest("Account Number should be a number");
            }
            // If accountNumber is not 1001, return HTTP 400
            if (accountNumber != 1001)
            {
                return BadRequest("Account Number should be 1001");
            }

            return Content($"Current Balance: {account.currentBalance}");
        }
    }
}
