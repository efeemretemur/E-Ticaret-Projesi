using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using Nora.Shop.Core.Entities;

namespace E_Ticaret_Projesi.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, UserManager<AppUser> userManager, IProductService productService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = _cartService.GetCartItems(userId!);
            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int productId, int quantity = 1, string? returnUrl = null)
        {
            var product = _productService.GetProductById(productId);
            if (product is null)
            {
                TempData["Error"] = "Ürün bulunamadı.";
                return RedirectToAction("Index", "Product");
            }

            if (product.Stock <= 0)
            {
                TempData["Error"] = "Bu ürün şu anda stokta bulunmuyor.";
                return RedirectToLocal(returnUrl);
            }

            quantity = Math.Max(1, quantity);
            if (quantity > product.Stock)
            {
                TempData["Error"] = $"Bu üründen en fazla {product.Stock} adet ekleyebilirsiniz.";
                quantity = product.Stock;
            }

            var userId = _userManager.GetUserId(User);
            _cartService.AddToCart(userId!, productId, quantity);
            TempData["Success"] = "Ürün sepete eklendi.";
            return RedirectToLocal(returnUrl, nameof(Index));
        }

        public IActionResult RemoveFromCart(int cartId)
        {
            _cartService.RemoveFromCart(cartId);
            TempData["Success"] = "Ürün sepetten kaldırıldı.";
            return RedirectToAction(nameof(Index));
        }

        private IActionResult RedirectToLocal(string? returnUrl, string fallbackAction = "Index")
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(fallbackAction);
        }
    }
}
