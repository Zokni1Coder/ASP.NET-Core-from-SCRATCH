using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace MiddlewareExample.CustomMiddleware
{
    //Nem fogjuk implementálni az IMiddleWare interface-t, helyette magunknak hozzuk létre a következő middleware-re mutató delegate-t, amit megkapp paraméterként a konstruktorban.
    public class HelloCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public HelloCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //Mindig az Invoke részbe tesszük az elvégzendő logikai műveleteket. Ha megnézed, az egyszerű Middleware classban is ide kerültek a műveletek.
        public async Task Invoke(HttpContext httpContext)
        {
            await httpContext.Response.WriteAsync("CustomMiddleware elkezdődött.<br>");

            StreamReader sr = new StreamReader(httpContext.Request.Body);
            string body = await sr.ReadToEndAsync();
            Dictionary<string, StringValues> QuerryString = new Dictionary<string, StringValues>();
            QuerryString = QueryHelpers.ParseQuery(body);

            if (QuerryString.Count > 0)
            {
                await httpContext.Response.WriteAsync($"Hello {QuerryString["firstName"]} {QuerryString["lastName"]}<br>");
            }

            await _next(httpContext);
            await httpContext.Response.WriteAsync("CustomMiddleware befejeződött.<br>");
        }
    }

    // Itt alkalmazzuk az Extension tudásunkat arra, hogy a pipeline-ba elhelyezzük az fenti osztályunkat. A pipe-linenak az a szerepe, hogy a framework futtatáskor összeállítja, majd adott sorrendben létrehozza és futtatja. Ez nélkül létrehozhatod a classt, de nem fogod tudni elérni.
    public static class HelloCustomMiddlewareExtensions
    {
        //Ezzel a függvénnyel ("UseHelloCustomMiddleware") fogod tudni elérni és futtatni a classt, azaz a middleware-t, ami egyben elhelyezi őt a pipeline-ba és végre is hajtja.
        public static IApplicationBuilder UseHelloCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HelloCustomMiddleware>();
        }
    }
}
