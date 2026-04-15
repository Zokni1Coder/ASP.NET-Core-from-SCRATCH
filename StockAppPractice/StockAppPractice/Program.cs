using Microsoft.Extensions.Configuration;
using Service;
using ServiceContract;
using StockAppPractice.OptionsPatterns;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<StockApiOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
