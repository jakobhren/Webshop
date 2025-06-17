using AdminSystem.Model.Entities;
using AdminSystem.Model.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        protected OrderItemRepository Repository { get; }

        public OrderItemController(OrderItemRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("{id}")]
        public ActionResult<OrderItem> GetOrderItem([FromRoute] int id)
        {
            OrderItem orderItem = Repository.GetOrderItemById(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderItem>> GetOrderItems()
        {
            return Ok(Repository.GetOrderItems());
        }

        [HttpPost("/api/orderitems")]
        public ActionResult CreateOrdemItem([FromBody] OrderItem orderItem)
        {
            if (orderItem == null)
            {
                return BadRequest("OrderItem info not correct");
            }
            int OrderItemId = Repository.CreateOrderItem(orderItem);

            return OrderItemId > 0 ? Ok(OrderItemId) : StatusCode(500, "Order creation failed.");
        }

        [HttpPut]
        public ActionResult UpdateOrderItem([FromBody] OrderItem orderItem)
        {
            if (orderItem == null)
            {
                return BadRequest("OrderItem info not correct");
            }
            OrderItem existingOrderItem = Repository.GetOrderItemById(orderItem.OrderItemId);
            if (existingOrderItem == null)
            {
                return NotFound($"OrderItem with id {orderItem.OrderItemId} not found");
            }
            bool status = Repository.UpdateOrderItem(orderItem);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Something went wrong while updating the order item.");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrderItem([FromRoute] int id)
        {
            OrderItem existingOrderItem = Repository.GetOrderItemById(id);
            if (existingOrderItem == null)
            {
                return NotFound($"OrderItem with id {id} not found");
            }
            bool status = Repository.DeleteOrderItem(id);
            if (status)
            {
                return NoContent();
            }
            return BadRequest($"Unable to delete order item with id {id}");
        }
    }
}
