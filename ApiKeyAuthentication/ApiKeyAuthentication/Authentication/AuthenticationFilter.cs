using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiKeyAuthentication.Authentication
{
    public class ApiKeyAuthenticationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.AuthenticationHeader,
                out var apiKey))
            {
                context.Result = new UnauthorizedObjectResult("Api Key is missing!!");
                return Task.CompletedTask;
            }

            var key = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>().
                      GetValue<string>(AuthConstants.ApiKeySectionName);

            if (key != apiKey)
            {
                context.Result = new UnauthorizedObjectResult("Invalid Api Key!!");
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
