var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Map("/", async context =>
{
    await context.Response.WriteAsync(app.Configuration["MyKey"] + "\n");

    await context.Response.WriteAsync(app.Configuration.GetValue<string>("MyKey") + "\n");

    //Ha nem találja a "Z" key-t, akkor defaultként a második paraméter jelenik meg.
    await context.Response.WriteAsync(app.Configuration.GetValue<string>("Z", "Key Z not found!") + "\n");

});

app.Run();