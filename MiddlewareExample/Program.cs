using Microsoft.AspNetCore.Mvc;
//importáld a környezetet/namespace-t, hogy el tudd érni az új mappát.
using MiddlewareExample.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
//A FirstMiddlewareClass beregisztrálása a Dependency Injection konténerbe. Az IMiddleware-t megvalósító osztályokat a framework innen hozza létre futásidőben, tehát minden egyes request egy új példányt jelent.
//Transient jelentése: Minden kéréshez új példány. 
//További értékek a Transient mellet: 
builder.Services.AddTransient<FirstMiddlewareClass>();
builder.Services.AddTransient<SecondMiddlewareClass>();
var app = builder.Build();


//Ahhoz, hogy tudjunk láncot használni "app.Use"-ra van szükségünk, aminek 2 paramétere van.Nyilván a context és egy delegate, ami rá fog mutatni a következő middleware-re.   
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync("Middleware 1 elkezdődött.<br>");
    await next(context);
    await context.Response.WriteAsync("Middleware 1 befejeződött.<br>");
});
//app.UseMiddleware<FirstMiddlewareClass>();
//Itt csak egyszerű metódusként meghívod az Extension-t!
//app.UseMyFirstExtension();



//Figyelj! Nem body-ból hanem url-ből veszem ki a query string értékeket.
//Az első lambda kifejezés az mindig a feltétel és utána jön az elvégzendő művelet. 
app.UseWhen(context => context.Request.QueryString.HasValue,
    app =>
    {
        app.UseHelloCustomMiddleware();

        app.Use(async(HttpContext context, RequestDelegate next) =>
        {
            await context.Response.WriteAsync("UseWhen branch-ben a kovetkezo middleware elkezdodott.<br>");
            await next(context);
            await context.Response.WriteAsync("UseWhen branch-ben a kovetkezo middleware befejezodot.<br>");
        });
    }
    );

//Az app.Run egy middleware, amiben elhelyeztünk egy lambda kifejezést, aminek egy paramétere van és végrehajtja a Szervusz! kiírást.,
//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Middleware 4 elkezdődött.<br>");
//    await context.Response.WriteAsync("Szervusz!<br>");
//    await context.Response.WriteAsync("Middleware 4 befejeződött.<br>");
//});

//Futtasd le így is. Miért nem jelenik meg a Szervusz megint! kiírás?
//Mert az első app.Run-ba fog befutni a request a klienstől, amit az első app.Run elvégez és visszaküldi a választ, de a második app.Run-nak nem továbbit semmit sem. Mindig az első app.Run-hoz fog befutni ebben az esetben a request mivel az van feljebb a sorokat nézve (ne feledd, ez is sorról-sorra fut).
//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Szervusz megint!<br>");
//});

app.Run();

