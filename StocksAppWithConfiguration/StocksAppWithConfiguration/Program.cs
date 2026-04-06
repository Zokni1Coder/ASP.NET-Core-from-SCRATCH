using Microsoft.Extensions.DependencyInjection;
using Service;
using ServiceContract;
using StocksAppWithConfiguration.OptionsPatterns;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.Configure<ConfigOptions>(builder.Configuration.GetSection("TradingOptions"));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
