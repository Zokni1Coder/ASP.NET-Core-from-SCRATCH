using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Login_App.MiddlewareClasses
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path == "/" && httpContext.Request.Method == "POST")
            {
                string email = "manager@example.com";
                string password = "manager-password";

                StreamReader sr = new StreamReader(httpContext.Request.Body);
                string body = await sr.ReadToEndAsync();
                Dictionary<string, StringValues> queryString = QueryHelpers.ParseQuery(body);

                if (!queryString.ContainsKey("email"))
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync("Invalid email.");
                }
                else if (!queryString.ContainsKey("password"))
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync("Invalid password.");
                }
                else if (queryString["email"] == email && queryString["password"] == password)
                {
                    httpContext.Response.StatusCode = 200;
                    await httpContext.Response.WriteAsync("Successfull login!");
                }
                else
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync("Invalid login!");
                }
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
