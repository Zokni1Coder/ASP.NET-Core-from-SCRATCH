using StockApp.OptionsPatterns;
using StockApp.ServiceContracts;
using StockApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
//Regisztráljuk a HttpClient infrastruktúrát (IHttpClientFactory) a DI containerben
builder.Services.AddHttpClient();
builder.Services.AddScoped<IStockApiService, StockApiService>();
builder.Services.Configure<StockApiOptions>(builder.Configuration.GetSection("StockAPI"));
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers ();

app.Run();
