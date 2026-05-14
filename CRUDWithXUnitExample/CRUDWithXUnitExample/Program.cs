using Services;
using ServiceContract;
using Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ICountryService, CountryService>();
builder.Services.AddSingleton<IPersonService, PersonService>();
//Megadjuk a IoC Poolba a db kapcsolatot.
builder.Services.AddDbContext<PersonsDbContext>(options =>
{
    //Mivel a DbContext univerzális, ezért megadjuk hogy SqlServer-rel fog együtt dolgozni.
    //Itt adjuk meg a Connection stringet is.
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnecion"));
});


//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
