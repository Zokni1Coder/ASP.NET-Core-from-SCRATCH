using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Net.Mime;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Run(async (HttpContext context) =>
{
    context.Response.Headers["RekaNeve"] = "Granyak Reka";
    context.Response.Headers["Server"] = "Reka";
    context.Response.StatusCode = 400;
    //Így adjuk meg a típusát és szépen le fogja kezelni. Nem kis/nagy betű érzékeny. Mivel ezt definiáltuk a developer toolsban is megjelenik a headersben.
    context.Response.Headers["content-type"] = "text/html";
    //Vagy így: context.Response.ContentType = "text/html";

    //Ugyanúgy a context-ől tudod a Request adatait elérni.
    string path = context.Request.Path;
    await context.Response.WriteAsync("<center><h1>Hello Reka!</h1></center>");
    await context.Response.WriteAsync("<ul><li>Granyak</li></ul>");
    await context.Response.WriteAsync($"A link: {path}");


    //context.Response.ContentType = "txt/html";
    switch (context.Request.Method)
    {
        case "GET" when context.Request.Query.Count() > 1:
            foreach (var item in context.Request.Query)
            {
                await context.Response.WriteAsync($"<br>Key:{item.Key}, Value: {item.Value}");
            }
            break;
        case "GET" when context.Request.Query.ContainsKey("id"):
            string id = context.Request.Query["id"];
            await context.Response.WriteAsync($"<br>A bevitt QuerryString: {id}");
            break;
        case "POST":
            //StreamReader mert Stream típusú a Body.
            StreamReader sr = new StreamReader(context.Request.Body);

            //Async, hogy olvasás közben nehogy elkezdjen valami más műveletet. Mivel Async, ezért meg kell várni, tehát kell az "await" kulcsszó.
            string body = await sr.ReadToEndAsync();

            //A Dictionary egy key-value párakat tartalmazó lista. Mivel a kulcsnevekből csak egy lehet, ezért string lesz. Tehát két "Age"-ből nekünk kulcsként csak egyet fog elmenteni. Mi lesz a két "Age" értékével? Össze lesz vonva egyetlen "Age" mező értékeként, ezért kell "StringValues" a "string" helyett. Lásd az eredményt.
            Dictionary<string, StringValues> queryBody = new Dictionary<string, StringValues>(); 

            //Átalakítod, azaz Parse-olod, mint egy egyszerű int esetében.
            queryBody = QueryHelpers.ParseQuery(body);

            foreach (var item in queryBody)
                await context.Response.WriteAsync($"<br>Dictionary Key: {item.Key}, Value(s): {item.Value}");
            break;
        default:
            await context.Response.WriteAsync("<br>Nincs QuerryString vagy nem \"id\" a kulcs mező!");
            break;
    }
});
app.Run();
