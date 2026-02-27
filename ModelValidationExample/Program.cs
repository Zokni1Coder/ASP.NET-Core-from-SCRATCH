using ModelValidationExample.CustomModelBinders;
using ModelValidationExample.Models;

var builder = WebApplication.CreateBuilder(args);
//Itt az AddControllers-nél hozzá kell adni a Providert, amit a pipeline-on végig haladva majd létre fog hozni.
builder.Services.AddControllers(option =>
{
    //Meg kell adni a pozícióját és a típusát.
    option.ModelBinderProviders.Insert(0, new PersonModelProvider());
});
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();


app.Run();
