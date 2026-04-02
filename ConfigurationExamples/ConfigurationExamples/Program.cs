using ConfigurationExamples;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//itt hozzáadjuk a konténerhez az Options Patternt alkalmazó osztályt, ami megfelel a konfiguráció szekciónak.
builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection("MasterKey"));

//Adjuk hozzá az új config  fájlt a builderhez
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    //Optional = ha nem találja akkor sem fog hibát dobni, mert opcionális, nem kötelező 
    //reloadOnChange = ha bármit változtatunk a custom configban újraindul az alkalmazás
    config.AddJsonFile("MyOwnConfig.json", optional: true, reloadOnChange: true);
});

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

//app.Map("/", async context =>
//{
//    await context.Response.WriteAsync(app.Configuration["MyKey"] + "\n");

//    await context.Response.WriteAsync(app.Configuration.GetValue<string>("MyKey") + "\n");

//    //Ha nem találja a "Z" key-t, akkor defaultként a második paraméter jelenik meg.
//    await context.Response.WriteAsync(app.Configuration.GetValue<string>("Z", "Key Z not found!") + "\n");

//});

app.Run();