
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddTransient<IWeatherService, Weath>
builder.Services.AddControllersWithViews();


var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
