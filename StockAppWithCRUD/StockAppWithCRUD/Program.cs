using Microsoft.Extensions.DependencyInjection;
using ServiceContract;
using Services;
using StockAppWithCRUD.Option_Pattern;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddSingleton<ITradeService, TradeService>();
builder.Services.Configure<Config_OptionPattern>(builder.Configuration.GetSection("FinnhubService"));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
