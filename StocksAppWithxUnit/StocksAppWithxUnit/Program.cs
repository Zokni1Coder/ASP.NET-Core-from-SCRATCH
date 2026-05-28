using Service;
using ServiceContract;
using StocksAppWithxUnit.OptionsPatterns;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.Configure<TradeOptions>(builder.Configuration.GetSection("ApiKey"));
var app = builder.Build();


app.UseStaticFiles();
app.MapControllers();

app.Run();
