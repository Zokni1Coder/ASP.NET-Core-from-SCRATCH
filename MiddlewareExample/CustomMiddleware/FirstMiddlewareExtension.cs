
using System.Threading.Tasks;

namespace MiddlewareExample.CustomMiddleware
{
    //Mivel extension-ről van szó, ezért muszáj hogy static legyen.
    public static class FirstMiddlewareExtension
    {
        //Az IApplicationBuilder gyakorlatilag egy Builder Design Pattern-t megvalósító felület (interface). Lényege, hogy egy objektum létre hoz előre meghatározott objektumokat. Például a MechanicBuilder objektum létrehozza a Car objektumot. Esetünkben azt jelenti, hogy az IApplicationBuilder kap egy újabb metódust, amit az általa létrehozott app-ba beletesz. 
        //A nevét illik Use szóval kezdeni!
        public static IApplicationBuilder UseMyFirstExtension(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecondMiddlewareClass>();
        }
    }
}
