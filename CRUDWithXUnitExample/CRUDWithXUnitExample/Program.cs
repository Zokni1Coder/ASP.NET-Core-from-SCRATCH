using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using ServiceContract;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IPersonService, PersonService>();
//Megadjuk a IoC Poolba a db kapcsolatot.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    //Mivel a DbContext univerzális, ezért megadjuk hogy SqlServer-rel fog együtt dolgozni.
    //Itt adjuk meg a Connection stringet is.
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnecion"));
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False
var app = builder.Build();
//Ha a Use.Rotativa()-t használom valamiért néha rövid időn belül leállítja a programot miután megkaptam új tabban a pdf fájlt. Hogy ezt elkerüld, használd a felette lévő kikommentelt részt.

//Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseRotativa();
app.UseStaticFiles();
app.MapControllers();

app.Run();
