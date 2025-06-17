using AdminSystem.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminSystem.Model;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AdminSystem.API.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase {
    
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login()
    {
        // Get the Authorization header
        var authHeader = Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic "))
        {
            return Unauthorized("Missing or invalid Authorization header");
        }

        try
        {
            // Decode credentials
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var decodedBytes = Convert.FromBase64String(encodedCredentials);
            var decodedCredentials = Encoding.UTF8.GetString(decodedBytes);

            // Split to email and password
            var credentials = decodedCredentials.Split(':');
            if (credentials.Length != 2)
            {
                return Unauthorized("Malformed credentials");
            }

            var email = credentials[0];
            var password = credentials[1];

            // Look up customer by email
            var customer = _context.customer.FirstOrDefault(c => c.Email == email);

            if (customer == null || customer.Password != password)
            {
                return Unauthorized("Invalid email or password");
            }

            // Optional: Return token or just confirmation
            return Ok(new
            {
                message = "Login successful",
                headerValue = $"Basic {encodedCredentials}",
                customer = new { customer.Email, customer.CustomerId }
            });
        }
        catch
        {
            return StatusCode(500, "An error occurred during login");
        }
    }
    }
}