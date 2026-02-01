using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//A Map-pon belül az url-ben lévő értéket a {} közé írjuk. NEM ÖSSZEKEVERNI A QUERY STRING-GEL! Ez nem kulcs-érték pár, csak szimpla routing.
app.Map("employee/{employeename}",async context =>
{
    //Így szeded ki az útvonalból a változót, a paramétert, ami egy Dictionary, ezért kell stringre alakítani.
    //A string után a "?" azért kell, mert nem lehet tudni, hogy a RouteValues() vissza fog-e adni értéket vagy null lesz. Ezzel a jellel engedélyezzük a null értéket is, azaz ezzel tesszük nullable értékké a mezőt.
    string? name = context.Request.RouteValues["employeename"].ToString();
    await context.Response.WriteAsync($"The searched profile is: {name}");
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
    await context.Response.WriteAsync($"Request received on the \"{context.Request.Path}\" path!");
});
app.Run();
