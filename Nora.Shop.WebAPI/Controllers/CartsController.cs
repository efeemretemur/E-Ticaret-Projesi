using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using Nora.Shop.WebAPI.Models;

namespace Nora.Shop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/cart")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult GetCart(string userId)
        {
            return Ok(_cartService.GetCartItems(userId));
        }

        [HttpPost]
        public IActionResult Add(string userId, AddToCartRequest request)
        {
            _cartService.AddToCart(userId, request.ProductId, request.Quantity);
            return Ok(_cartService.GetCartItems(userId));
        }

        [HttpDelete("{cartId}")]
        public IActionResult Remove(int cartId)
        {
            _cartService.RemoveFromCart(cartId);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Clear(string userId)
        {
            _cartService.ClearCart(userId);
            return NoContent();
        }
    }
}
