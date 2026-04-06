using Microsoft.Extensions.Options;
using Social_Media_Links.Options_Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<SocialMediaLinksOptions>(builder.Configuration.GetSection("SocialMediaLinks"));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
