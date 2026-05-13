using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;

namespace E_Ticaret_Projesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private static readonly string[] StatusOptions =
        {
            "Beklemede",
            "Hazırlanıyor",
            "Kargoda",
            "Teslim Edildi",
            "İptal Edildi"
        };

        private readonly IOrderService _orderService;

        public AdminOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            ViewBag.StatusOptions = StatusOptions;
            return View(_orderService.GetAllOrders());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            if (!StatusOptions.Contains(status))
            {
                TempData["Error"] = "Geçersiz sipariş durumu seçildi.";
                return RedirectToAction(nameof(Index));
            }

            _orderService.UpdateOrderStatus(id, status);
            TempData["Success"] = "Sipariş durumu güncellendi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
