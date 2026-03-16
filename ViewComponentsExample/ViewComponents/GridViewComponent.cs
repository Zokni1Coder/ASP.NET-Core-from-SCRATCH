
using Microsoft.AspNetCore.Mvc;
using ViewComponentsExample.Models;

namespace ViewComponentsExample.ViewComponents
{
    //Itt vigyázz mert összekeverhet a C# intelli sense. Nem ViewComponent"S",
    //se nem ViewComponentExample, hanem ViewComponent és akkor le tudod a névteret
    //is generáltatni vele.

    //Két féle képpen tudjuk itt is megjelölni a szülőösztályt: 
    //1.) így ahogy teszem (fontos hogy a név az ViewComponent-re fejeződjön)
    //2.) [ViewComponent] attribútummal az osztály felett és az osztály név mindegy.
    public class GridViewComponent : ViewComponent
    {
        /*
         Azért használunk Task-ot, hogy ne terheljük le a szervert, ne raboljuk az idejét. A többi előnyét konkrétabban a füzetben kerest. 
         */
        public async Task<IViewComponentResult> InvokeAsync(Manufacturer manufacturer)
        {
            //Manufacturer manufacturer1 = new Manufacturer()
            //{
            //    Brand = "Zastava",
            //    Models = new List<CarModel>()
            //    {
            //        new CarModel()
            //        {
            //            Chassie = Chassis.limousine,
            //            Model = "101"
            //        },
            //        new CarModel()
            //        {
            //            Chassie = Chassis.hothatch,
            //            Model = "Yugo"
            //        },
            //        new CarModel()
            //        {
            //            Chassie = Chassis.limousine,
            //            Model = "128"
            //        }
            //    }
            //};

            ViewData["Manufacturer"] = manufacturer;

            /*
              Ha csak "return View();" akkor a defaul útvonal és fájl: Views/Shared/Components/Grid/Default.cshtml
             */

            //Ugyanúgy adjuk át az objektumot mint eddig a többi View típusoknál.
            return View("sample", manufacturer);
        }
    }
}
