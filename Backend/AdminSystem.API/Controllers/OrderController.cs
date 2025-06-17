using AdminSystem.Model.Entities;
using AdminSystem.Model.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        protected OrderRepository Repository { get; }
        protected OrderItemRepository OrderItemRepo { get; }

        public OrderController(OrderRepository repository, OrderItemRepository orderItemRepo)
        {
            Repository = repository;
            OrderItemRepo = orderItemRepo;
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder([FromRoute] int id)
        {
            Order order = Repository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return Ok(Repository.GetOrders());

        }

        [HttpGet("/api/orders/{email}")]
        public IActionResult GetCustomerOrders(string email)
        {
             var order = Repository.GetCustomerOrders(email); // Assuming the repository has a method like this

            if (order == null)
            {
                return NotFound($"Order with email {email} not found.");
            }

            return Ok(order);

           
        }

        [HttpPost("/api/orders")]
        public ActionResult CreateOrder([FromBody] Order order)
        {
           if (order == null || order.OrderItems == null || order.OrderItems.Count == 0)
            {
                return BadRequest("Order info is incorrect or order items are missing");
            }
            
            int orderId = Repository.CreateOrder(order);
            if (orderId <= 0)
            {
                return StatusCode(500, "Order creation failed.");
            }
            foreach (var item in order.OrderItems)
            {
                item.OrderId = orderId;
                var result = OrderItemRepo.CreateOrderItem(item);
                if (result <= 0)
                {
                    return StatusCode(500, "Failed to create one or more order items.");
                }
            }

            return Ok(orderId);
            
        }

        [HttpPut]
        public ActionResult UpdateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order info not correct");
            }
            Order existingOrder = Repository.GetOrderById(order.OrderId);
            if (existingOrder == null)
            {
                return NotFound($"Order with id {order.OrderId} not found");
            }
            bool status = Repository.UpdateOrder(order);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Something went wrong");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrder([FromRoute] int id)
        {
            Order existingOrder = Repository.GetOrderById(id);
            if (existingOrder == null)
            {
                return NotFound($"Order with id {id} not found");
            }
            bool status = Repository.DeleteOrder(id);
            if (status)
            {
                return NoContent();
            }
            return BadRequest($"Unable to delete order with id {id}");
        }
    }
}
