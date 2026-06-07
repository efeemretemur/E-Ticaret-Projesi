using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using Nora.Shop.WebAPI.Models;

namespace Nora.Shop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_orderService.GetAllOrders());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _orderService.GetOrderById(id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetUserOrders(string userId)
        {
            return Ok(_orderService.GetUserOrders(userId));
        }

        [HttpPost("user/{userId}")]
        public IActionResult Create(string userId, CreateOrderRequest request)
        {
            var created = _orderService.CreateOrder(userId, request.Address);
            return created ? Ok("Siparis olusturuldu.") : BadRequest("Sepet bos veya stok yetersiz.");
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, UpdateOrderStatusRequest request)
        {
            var order = _orderService.GetOrderById(id);
            if (order is null)
            {
                return NotFound();
            }

            _orderService.UpdateOrderStatus(id, request.Status);
            return NoContent();
        }
    }
}
