
using System.Net;

namespace ApiKeyAuthentication.Authentication
{
    public class AuthenticationMiddleware : IMiddleware
    {
        public readonly IConfiguration _configuration;

        public AuthenticationMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }
     
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Headers.TryGetValue(AuthConstants.AuthenticationHeader,
                out var apiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Api Key is missing!!");
                return;
            }

            var key = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

            if (key != apiKey)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid Api Key!!");
                return;
            }

            await next(context);
        }
    }
}
