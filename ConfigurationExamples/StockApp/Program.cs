var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
//Regisztráljuk a HttpClient infrastruktúrát (IHttpClientFactory) a DI containerben
builder.Services.AddHttpClient();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers ();

app.Run();
