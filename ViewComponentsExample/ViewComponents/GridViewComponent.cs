
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            /*
              Ha csak "return View();" akkor a defaul útvonal és fájl: Views/Shared/Components/Grid/Default.cshtml
             */
             return View("sample");
        }
    }
}
