using Services;
using ServiceContract;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICountryService, CountryService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
