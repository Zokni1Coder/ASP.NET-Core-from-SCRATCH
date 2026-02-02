using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//A Map-pon belül az url-ben lévő értéket a {} közé írjuk. NEM ÖSSZEKEVERNI A QUERY STRING-GEL! Ez nem kulcs-érték pár, csak szimpla routing.
//Itt jól látható a default paraméter. Ha érkezik paraméter, akkor figyelmen kívül hagyja a defaultot.
//FONTOOOOOS: NE HAGYJ HELYET A DEFAULT ÉRTÉKNÉL! 
//app.Map("employee/{employeename=Reka}", async context =>
//{
//    //Így szeded ki az útvonalból a változót, a paramétert, ami egy Dictionary, ezért kell stringre alakítani.
//    //A string után a "?" azért kell, mert nem lehet tudni, hogy a RouteValues() vissza fog-e adni értéket vagy null lesz. Ezzel a jellel engedélyezzük a null értéket is, azaz ezzel tesszük nullable értékké a mezőt.
//    string? name = context.Request.RouteValues["employeename"].ToString();
//    await context.Response.WriteAsync($"The searched profile is: {name}");
//});



//A "name" paraméter utáni "?" jelzi az opcionális mivoltát. Gyakorlatban ezt leggyakrabban az adatbázissal történő kommunikációnál szoktuk alkalmazni. Van paraméter?!, ha van végrehajtuk, ha nem akkor nem terheljük a kapcsolatot. 
app.Map("employee/{name?}", async context =>
{
    if (context.Request.RouteValues.ContainsKey("name"))
    {
        string name = context.Request.RouteValues["name"].ToString();
        await context.Response.WriteAsync($"The name of the employee is: {name}");
    }
    else
        await context.Response.WriteAsync("The name of the employee is: not supplied!");
});

//Itt két megszorítást alkalmazunk: az id-re és a startingDate-re. Az egyik int típusú és az érték 0 vagy nagyobb lehet, a másik dátum típusú kell, hogy legyen. Mint ahogy láthatod egy paraméterre több megszorítást is alkalmazhatsz.
//Sablon:     {mezőNév:típus}
//Ha ezeknek nem tesz eleget a kérés, akkor nem fog lefutni ez az endpoint. PL: "employee/abc/2022-06-22" esetében nem fog match-elni, tehát nem fog lefutni, mert az id helyére az került, hogy "abc".
//Fun-fact: ha nem megfelelően írod a dátumot, akkor sem fog matchelni. PL: "employee/abc/2022-20-22", mert nincs olyan, hogy 20. hónap!  
app.Map("employee/{id:int:min(0)}/{startingDate:datetime}", async context =>
{
    DateTime startingDate = Convert.ToDateTime(context.Request.RouteValues["startingDate"]);
    int id = Convert.ToInt32(context.Request.RouteValues["id"]);
    await context.Response.WriteAsync($"The employee (ID: {id}) has/will started/starting on: {startingDate}");
});

//GUID létrehozása: Tools -> Create GUID -> én a 4. opciót választottam ({7FDC069B-AF10-46E5-B939-D57C750B50F3})
//FONTOS: az url-be a kapcsos zárójeleket töröld ki, tehát: 7FDC069B-AF10-46E5-B939-D57C750B50F3
app.Map("employee/{id:guid}", async context =>
{
    //Mivel a Guid.Parse egy stringet vár paraméterül, ezért először stringgé alakítjuk.
    Guid guid = Guid.Parse(Convert.ToString(context.Request.RouteValues["id"])!);
    await context.Response.WriteAsync($"The ID of the employee is: {guid}");
});

//Egy regex példa arra, hogy elfogadja a hónap rövidítéseit angolul írva. Nem mindig felsorolással működik és ajánlott. Nézz neten utána. Rengeteg van!
app.Map("employee/{id:int:min(0)}/{month:regex(^(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)$)}", async context =>
{
    await context.Response.WriteAsync("This is the route with regex!");
});

//Első paraméter az útvonal, az úgynevezett "path", majd utána a lambda kifejezés. Tehát ezt az "endpointot" a "localhost:XXXX/home" url-lel tudod elérni bármilyen metódussal.
app.Map("home", async (context) =>
{
    await context.Response.WriteAsync("Welcome on the Home page!");
});

app.Map("about", async (context) =>
{
    await context.Response.WriteAsync("Welcome on the About page!");
});

//Ez ugyanaz, mint az első "endpoint", csak ez akkor fog lefutni amikor a metódus "POST" és az url "localhost:XXXX/home".
app.MapPost("home", async (context) =>
{
    await context.Response.WriteAsync("Welcome on the Home page with POST request.");
});

//Ez egy speciális endpoint, amely akkor fut le, ha egyik korábban definiált endpoint sem illeszkedik a kérésre. Úgy kell elképzelni, mint a try-catch blokkban az utolsó catch, ami általános éss minden error-t megfog. Ez minden kérést megfog.
app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync($"No route matched at: \"{context.Request.Path}\" path!");
});
app.Run();
