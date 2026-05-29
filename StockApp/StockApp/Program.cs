using Entities;
using Microsoft.Extensions.DependencyInjection;
using ServiceContract;
using ServiceContract.Option_Pattern;
using Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddSingleton<ITradeService, TradeService>();
builder.Services.Configure<Config_OptionPattern>(builder.Configuration.GetSection("FinnhubService"));
builder.Services.AddDbContext<StockMarketDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings").GetValue<string>("DefaultConnection"))
);

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();