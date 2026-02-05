using Countries_App.RouteConstraintClasses;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(option =>
{
    option.ConstraintMap.Add("ID", typeof(countryIDConstraint));
});
var app = builder.Build();

Dictionary<int, StringValues> countries = new Dictionary<int, StringValues>();
countries.Add(1, "United States");
countries.Add(2, "Canada");
countries.Add(3, "United Kingdom");
countries.Add(4, "India");
countries.Add(5, "Japan");

app.MapGet("countries/{countryID:int:ID}", async context =>
{
    int countryID = Convert.ToInt32(context.Request.RouteValues["countryID"]);
    if (countryID > 100)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The CountryID should be between 1 and 100<br>");
    }
    else
    {
        if (countries.ContainsKey(countryID))
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync($"ID: {countryID}, Country: {countries[countryID]}<br>");
        }
    }    
});

app.MapGet("countries", async context =>
{
    context.Response.StatusCode = 200;
    foreach (var country in countries)
    {
        await context.Response.WriteAsync($"ID: {country.Key}, Country: {country.Value}<br>");
    }
});

app.MapFallback(async context =>
{
    context.Response.StatusCode = 404;
    await context.Response.WriteAsync("Not found!");
});

app.Run();
