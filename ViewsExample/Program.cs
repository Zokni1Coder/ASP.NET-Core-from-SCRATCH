var builder = WebApplication.CreateBuilder(args);

//Mivel nem csak Controllerek, hanem View-jaink is lesznek, ezért együtt adjuk őket meg. Miért?! Mert össze fognak tartozni. Lásd: HomeController kommentek. 
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
