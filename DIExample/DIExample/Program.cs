using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//Itt adjuk hozzá az IoC konténerhez a Service-ünket.
//builder.Services.Add(new ServiceDescriptor(
//    typeof(ICitiesService),
//    typeof(CitiesService),
//    //ServiceLifetime.Transient    //Transient/Scoped/Singleton
//    //Itt állítsd át az életciklusát Scoped-ra.
//    ServiceLifetime.Scoped
//    ));

//A fenti kódnak, amivel a konténerhez adunk egy service-t, van rövidebb változata is:
/*
    builder.Services.AddTransient<ICitiesService, CitiesService>();
    builder.Services.AddSingleton<ICitiesService, CitiesService>();
*/
builder.Services.AddScoped<ICitiesService, CitiesService>();


var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
