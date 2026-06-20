using E_Ticaret_Projesi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using Nora.Shop.Core.Entities;

namespace E_Ticaret_Projesi.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(IOrderService orderService, ICartService cartService, UserManager<AppUser> userManager)
        {
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = _cartService.GetCartItems(userId!);
            if (cartItems.Count == 0)
            {
                TempData["Error"] = "Sipariş oluşturmadan önce sepetinize ürün eklemelisiniz.";
                return RedirectToAction("Index", "Cart");
            }

            var user = await _userManager.GetUserAsync(User);
            var model = new CheckoutViewModel
            {
                Address = user?.Address ?? string.Empty,
                CartItems = cartItems
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteOrder(CheckoutViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = _cartService.GetCartItems(userId!);
            if (cartItems.Count == 0)
            {
                TempData["Error"] = "Sepetiniz boş olduğu için sipariş oluşturulamadı.";
                return RedirectToAction("Index", "Cart");
            }

            model.CartItems = cartItems;

            if (!ModelState.IsValid)
            {
                return View("Checkout", model);
            }

            var created = _orderService.CreateOrder(userId!, model.Address);
            if (!created)
            {
                ModelState.AddModelError(string.Empty, "Sepette stok problemi olan ürünler var. Lütfen sepetinizi kontrol edin.");
                model.CartItems = _cartService.GetCartItems(userId!);
                return View("Checkout", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is not null && user.Address != model.Address)
            {
                user.Address = model.Address;
                await _userManager.UpdateAsync(user);
            }

            TempData["Success"] = "Siparişiniz başarıyla oluşturuldu.";
            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess() => View();

        public IActionResult MyOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetUserOrders(userId!);
            return View(orders);
        }
    }
}
