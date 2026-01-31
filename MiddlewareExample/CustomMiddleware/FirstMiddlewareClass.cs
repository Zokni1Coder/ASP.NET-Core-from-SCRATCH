namespace MiddlewareExample.CustomMiddleware
{
    //Meg kell adni az IMiddleware interface-t(felület).
    public class FirstMiddlewareClass : IMiddleware
    {
        //Az IMiddleware interface-ből fakad az InvokeAsync Task, ami egy egy aszinkron művelet állapotát és életciklusát leíró objektum. Lehetővé teszi, hogy a framework megvárja, kezelje és láncolja a middleware-ek futását.
        //Task állapotai: Running, CompletedSuccessfully, Faulted (exception), Canceled.
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //logikai rész (a következő middleware futtatása előtt fut le)
            await context.Response.WriteAsync("Middleware 2 elkezdődött.<br>");
            await next(context);
            //logikai rész (a következő middleware futtatása után fut le (5.kep-et nézve amikor már visszafelé haladunk a response felé).
            await context.Response.WriteAsync("Middleware 2 befejeződött.<br>");
        }
    }
}
