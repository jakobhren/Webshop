namespace AdminSystem.API.Middleware;
public class HeaderAuthenticationMiddleware {
    private const string MY_SECRET_VALUE = "Abc123!!!";
    private readonly RequestDelegate _next;
    public HeaderAuthenticationMiddleware(RequestDelegate next) {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context) {

        // 1. Try to retrive the Request Header containing our secret value
        string? authHeaderValue = context.Request.Headers["X-My-Request-Header"];

        // 2. if not found, then return with Unauthorized response
        if (string.IsNullOrWhiteSpace(authHeaderValue)) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Auth Header value not provided");
            return;
        }

        // 3. If the secret value is NOT correct, return with Unauthorized response
        if (!string.Equals(authHeaderValue, MY_SECRET_VALUE)) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Auth Header value incorrect");
            return;
        }

        // 4. Continue with the request
        await _next(context);
        }
        }
        
public static class HeaderAuthenticationMiddlewareExtensions {
    public static IApplicationBuilder UseHeaderAuthenticationMiddleware(this IApplicationBuilder builder) {
        return builder.UseMiddleware<HeaderAuthenticationMiddleware>();
    }
}


// other code
/*
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace AdminSystem.API.Middleware;
public class BasicAuthenticationMiddleware {
    // Ideally, we would want to verfy them against a database
    private const string EMAIL = "john@doe";
    private const string PASSWORD = "VerySecret!";
    private readonly RequestDelegate _next;
    public BasicAuthenticationMiddleware(RequestDelegate next) {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context) {
        
        // Bypass authentication for [AllowAnonymous]
        if (context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null) {
            await _next(context);
            return;
        }
        // 1. Try to retrieve the Request Header containing our secret value
        string? authHeader = context.Request.Headers["Authorization"];

        // 2. If not found, then return with Unauthrozied response
        if (authHeader == null) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Authorization Header value not provided");
            return;
        }

        // 3. Extract the username and password from the value by splitting it on space,
        // as the value looks something like 'Basic am9obi5kb2U6VmVyeVNlY3JldCE='
        var auth = authHeader.Split([' '])[1];

        // 4. Convert it form Base64 encoded text, back to normal text
        var emailAndPassword = Encoding.UTF8.GetString(Convert.FromBase64String(auth));

        // 5. Extract username and password, which are separated by a semicolon
        var email = emailAndPassword.Split([':'])[0];
        var password = emailAndPassword.Split([':'])[1];

        // 6. Check if both username and password are correct
        if (email == EMAIL && password == PASSWORD) {
            await _next(context);
        }
        else {
            // If not, then send Unauthorized response
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Incorrect credentials provided");
            return;
        }
    }
}

public static class BasicAuthenticationMiddlewareExtensions {
    public static IApplicationBuilder UseBasicAuthenticationMiddleware(this
        IApplicationBuilder builder) {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
}
*/