using Service;
using ServiceContract;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
var app = builder.Build();


app.UseStaticFiles();
app.MapControllers();

app.Run();
