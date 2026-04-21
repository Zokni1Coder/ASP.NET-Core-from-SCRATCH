using Services;
using ServiceContract;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ICountryService, CountryService>();
builder.Services.AddSingleton<IPersonService, PersonService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
