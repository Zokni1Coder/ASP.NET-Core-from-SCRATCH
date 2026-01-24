
namespace MiddlewareExample.CustomMiddleware
{
    //Ne felejtsd el a Program.cs-ben megadni a Services-ben!
    public class SecondMiddlewareClass : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("Middleware 3 elkezdődött.<br>");
            await next(context);
            await context.Response.WriteAsync("Middleware 3 befejeződött.<br>");
        }
    }
}
