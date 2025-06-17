using AdminSystem.Model.Entities;
using AdminSystem.Model.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CustomerController : ControllerBase
        {
        protected CustomerRepository Repository {get;}

        private readonly CustomerRepository _repository;
        
        public CustomerController(CustomerRepository repository) {
            Repository = repository;
        }
            
        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer([FromRoute] int id)
        {
            var customer = HttpContext.Items["customer"] as Customer;
            if (customer == null) {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpGet]
         [AllowAnonymous]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            return Ok(Repository.GetCustomers());
        }
          // PUBLIC endpoint for email checking (no auth required)
        [HttpGet("/api/public/email-exists/{email}")]
        [AllowAnonymous] 
        public IActionResult CheckEmailPublic(string email)
        {
            bool exists = Repository.GetCustomerByEmail(email) != null;
            return Ok(new { exists });
        }

        [HttpGet("/api/customer/details/{email}")]
        [AllowAnonymous] // No authentication required for this endpoint
        public ActionResult<Customer> GetCustomerDetails(string email)
        {
            var customer = Repository.GetCustomerDetails(email); // Assuming the repository has a method like this

            if (customer == null)
            {
                return NotFound($"Customer with email {email} not found.");
            }

            return Ok(customer);
        }

        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateCustomer([FromBody] Customer customer) {
            // Check if customer already exists by email
            var existingCustomer = Repository.GetCustomerByEmail(customer.Email);
            if (existingCustomer != null)
            {
                return Conflict("A customer with this email already exists.");
            }

            // Insert the customer
            var customerId = Repository.InsertCustomer(customer);
            if (customerId.HasValue)
            {
                customer.CustomerId = customerId.Value;
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
            }

            return BadRequest("Failed to create customer.");
            /*
            if (customer == null)
            {
                return BadRequest("Customer data is required.");
            }

            var customerid = Repository.InsertCustomer(customer);
            if (customerid.HasValue)
            {
                //return Ok(customer);
                customer.CustomerId = customerid.Value;
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
            }
            
            return BadRequest(); */
        }
        [HttpPut]
        public ActionResult UpdateCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer info not correct");
            }
            Customer existinCustomer = Repository.GetCustomerById(customer.CustomerId);
            if (existinCustomer == null)
            {
                return NotFound($"Customer with id {customer.CustomerId} not found");
            }
            bool status = Repository.UpdateCustomer(customer);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Something went wrong");
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer([FromRoute] int id) {

            Customer existingCustomer = Repository.GetCustomerById(id);
            if (existingCustomer == null)
            {
                return NotFound($"Customer with id {id} not found");
            }
            bool status = Repository.DeleteCustomer(id);
            if (status)
            {
                return NoContent();
            }
            return BadRequest($"Unable to delete customer with id {id}");
        }
    }
}
