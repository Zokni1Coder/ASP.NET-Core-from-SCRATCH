using StockApp.OptionsPatterns;
using StockApp.ServiceContracts;
using StockApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
//Regisztráljuk a HttpClient infrastruktúrát (IHttpClientFactory) a DI containerben.
builder.Services.AddHttpClient();
//Regisztráljuk a saját service-ünket scope-ként a container-be.
builder.Services.AddScoped<IStockApiService, StockApiService>();
//Regisztráljuk a konfigot a container-be.
builder.Services.Configure<StockApiOptions>(builder.Configuration.GetSection("StockAPI"));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers ();

app.Run();
