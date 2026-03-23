using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        //Létrehozol egy readonly mezőt a Service-ből.
        //DIP-et betartva, a mező, az egy Interface lesz, amit a Service implementál. Ahhoz, hogy ez működjön, állítsd be a DIExmaple-nek a a ServiceContracts-ot mint Dependency. 
        private readonly ICitiesService _citiesService;
        //Ahhoz, hogy új Scope-ot ttudjunk létrehozni egy Factory tervezési mintájú interface-t kell példányosítanunk.
        private readonly IServiceScopeFactory _scopeFactory;
        //Paraméterül kap egy ICitiesService-t, de a IoC Container egy CitiesService egyedet fog küldeni és az kerül bele a mezőbe.
        public HomeController(ICitiesService citiesService, IServiceScopeFactory scopeFactory)
        {
            //A mezőnek létrehozol egy egyedet. Ez nem DI, de ezzel majd később foglalkozunk.
            //new CitiesService();
            this._citiesService = citiesService;
            this._scopeFactory = scopeFactory;
        }

        //Megjelöljük a paramétert a "[FromServices]" attribútummal, hogy az ASP.Net Core tudja hogy az IoC Container-nek ide egy objekttumot kell hogy szolgáltassson. Alapessetben csak a konstruktorokat figyeli.
        [Route("/")]
        public IActionResult Index([FromServices] ICitiesService citiesService)
        {
            //Így el tudjuk érni a Service metódusát és értéket fogunk kapni.
            //Mivel módosítottuk a CitiesService-t és nem gett property van, hanem metódus, ezért itt metódusként fogjuk meghívni.

            //Meghívjük a paraméterként kapott Service osztály metódusát.
            List<string> cities = citiesService.GetCities();
            ViewBag.InstanceID_CitiesService_InScope = citiesService.GetGuid();

            //Itt a usingban az IServiceScope és az IServiceScopeFactory segítségével létrehozunk egy új scope-ot. Azért kell a using(), mert a végén autómatikusan történik a Dispose.
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                //Inject CitiesService
                ICitiesService citiesServiceChildScope = scope.ServiceProvider.GetService<ICitiesService>(); //Ez azért számít injektion-nek mert mi csak egy interface-t jelölünk ki, a framework fogja nekünk auttómatikusan a megfelelő típusú példányt létrehozni és átadni.

                ViewBag.InstanceID_CitiesService_InChildScope = citiesServiceChildScope.GetGuid();
            }  //Itt a "}" jelnél történik minden Dispose()
            return View(cities);
        }
    }
}
