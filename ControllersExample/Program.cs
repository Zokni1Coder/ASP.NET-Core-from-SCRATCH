var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); //Ez hozzáadja autómatikusan az ÖSSZES controller osztályt a Services Collection-be, mint Service!

var app = builder.Build();

app.MapControllers(); //Ezzel engedélyezzük, hogy az URL-ben minden kontrollert és metódusát el tudjuk érni.

app.Run();
