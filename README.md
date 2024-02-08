# Authentication Mechanism with API Key

This repository demonstrates two approaches for API key authentication in a web API: Middleware and Auth Filter.

## API Key Authentication

To interact with our API, you need to include an API key in your requests as part of header. The API key serves as a secure and easy way to authenticate and authorize your requests.

# API Key Authentication for Web API Requests

## Middleware Approach

   ```csharp
   // ApiKeyMiddleware.cs
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
 ```

## Authorization Filter Approach
 ```csharp
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
 ```

### Including the API Key in Requests

When making requests to our API, include the API key in the request header as follows:

```http
GET /api/endpoint
Host: api.example.com
X-API-KEY: ApiKey YOUR_API_KEY

