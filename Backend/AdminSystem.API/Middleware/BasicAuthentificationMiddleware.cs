using System.Text;
using AdminSystem.Model;
using AdminSystem.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace AdminSystem.API.Middleware
{
    public class BasicAuthenticationMiddleware : IMiddleware
    {
         
        private readonly AppDbContext _dbContext;
        

        public BasicAuthenticationMiddleware( AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            
            // Bypass authentication for [AllowAnonymous]
            if (context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                await next(context);
                return;
            }

            // 1. Try to retrieve the Request Header containing our secret value
            string? authHeader = context.Request.Headers["Authorization"];

            // 2. If not found, then return with Unauthorized response
            if (string.IsNullOrEmpty(authHeader))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization Header value not provided");
                return;
            }

            // 3. Basic auth header should be in the form of "Basic <base64_encoded_credentials>"
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Invalid Authorization header format.");
                return;
            }

            // 4. Extract the base64-encoded credentials (strip "Basic " prefix)
            var auth = authHeader.Substring(6).Trim(); // Remove "Basic " from the header value

            try
            {
                // 5. Decode the Base64 string to get the email:password
                var emailAndPassword = Encoding.UTF8.GetString(Convert.FromBase64String(auth));

                // 6. Split the decoded string to get email and password
                var credentials = emailAndPassword.Split(':');
                if (credentials.Length != 2)
                {
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("Invalid credentials format.");
                    return;
                }

                var email = credentials[0];
                var password = credentials[1];

                // Check if the user exists in the database
                 var customer = await _dbContext.customer.FirstOrDefaultAsync(c => c.Email == email);

                // 7. Check if both email and password are correct
                if (customer == null)
                {
                 context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("User not found.");
                    return;
                }
                
                if (customer.Password != password)
                {
                    // If credentials are incorrect, send Unauthorized response
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Incorrect credentials provided");
                    return;
                }
                 // Authenticated, continue
                await next(context);
            }
            catch (FormatException)
            {
                // If Base64 decoding fails
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Invalid Base64 encoding in Authorization header.");
            }
        }
    }

    // Extension method for adding the middleware to the pipeline
    public static class BasicAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}
