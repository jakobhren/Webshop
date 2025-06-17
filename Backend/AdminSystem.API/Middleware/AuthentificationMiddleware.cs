using System.Text;
using Microsoft.AspNetCore.Authorization;
using AdminSystem.Model.Repositories;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public AuthenticationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }

        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader)
            && authHeader.ToString().StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var encoded = authHeader.ToString().Substring("Basic ".Length).Trim();
                var decodedBytes = Convert.FromBase64String(encoded);
                var decoded = Encoding.UTF8.GetString(decodedBytes);

                var parts = decoded.Split(':', 2); // Ensure only one split on first colon
                if (parts.Length != 2)
                {
                    await Reject(context, "Malformed credentials.");
                    return;
                }

                var email = parts[0];
                var password = parts[1];

                using var scope = _scopeFactory.CreateScope();
                var customerRepository = scope.ServiceProvider.GetRequiredService<CustomerRepository>();
                var customer = customerRepository.GetCustomerByEmail(email);

                if (customer != null && customer.Password == password)
                {
                    // Add customer to HttpContext so it can be accessed in controllers
                    context.Items["Customer"] = customer;
                    await _next(context);
                    return;
                }
            }
            catch (FormatException)
            {
                await Reject(context, "Invalid Base64 encoding.");
                return;
            }
            catch (Exception ex)
            {
                // You can log the exception here
                await Reject(context, "Authentication error.");
                return;
            }
        }

        await Reject(context, "Missing or invalid Authorization header.");
    }

    private Task Reject(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return context.Response.WriteAsync($"Unauthorized: {message}");
    }
}
