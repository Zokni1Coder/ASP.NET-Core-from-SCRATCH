using Login_App.MiddlewareClasses;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    if (context.Request.Method == "GET")
    {
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("Welcome!");
    }
    await next(context);
});

app.UseLoginMiddleware();

app.Run();



//Első megoldás MiddlewareClass nélkül.
//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    if (context.Request.Method == "GET")
//    {
//        context.Response.StatusCode = 200;
//        await context.Response.WriteAsync("Welcome!");
//    }
//    await next(context);
//});

//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    if (context.Request.Method != "POST")
//    {
//        await next(context);
//        return;
//    }

//    StreamReader sr = new StreamReader(context.Request.Body);
//    string body = await sr.ReadToEndAsync();
//    Dictionary<string, StringValues> queryString = QueryHelpers.ParseQuery(body);

//    if (!queryString.ContainsKey("password"))
//    {
//        context.Response.StatusCode = 400;
//        await context.Response.WriteAsync("Invalid password.");
//        return;
//    }
//    if (!queryString.ContainsKey("email"))
//    {
//        context.Response.StatusCode = 400;
//        await context.Response.WriteAsync("Invalid email.");
//        return;
//    }
//    if (queryString["email"] == "manager@example.com" && queryString["password"] == "manager-password")
//    {
//        context.Response.StatusCode = 200;
//        await context.Response.WriteAsync("Successful login");
//        return;
//    }
//    context.Response.StatusCode = 400;
//    await context.Response.WriteAsync("Invalid login");
//});
//app.Run();

