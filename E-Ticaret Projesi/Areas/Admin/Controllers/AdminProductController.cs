using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using Nora.Shop.Core.Entities;

namespace E_Ticaret_Projesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public AdminProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            ViewBag.CategoryMap = _categoryService.GetAllCategories().ToDictionary(c => c.Id, c => c.Name);
            return View(_productService.GetAllProducts());
        }

        public IActionResult Create()
        {
            if (!LoadCategories())
            {
                TempData["Error"] = "Ürün eklemeden önce en az bir kategori oluşturmalısınız.";
                return RedirectToAction("Create", "AdminCategory", new { area = "Admin" });
            }

            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                LoadCategories();
                return View(product);
            }

            _productService.AddProduct(product);
            TempData["Success"] = "Ürün başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product is null)
            {
                TempData["Error"] = "Düzenlemek istediğiniz ürün bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            LoadCategories();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                LoadCategories();
                return View(product);
            }

            _productService.UpdateProduct(product);
            TempData["Success"] = "Ürün bilgileri güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product is null)
            {
                TempData["Error"] = "Silmek istediğiniz ürün bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            _productService.DeleteProduct(id);
            TempData["Success"] = "Ürün silindi.";
            return RedirectToAction(nameof(Index));
        }

        private bool LoadCategories()
        {
            var categories = _categoryService.GetAllCategories();
            ViewBag.Categories = categories;
            return categories.Any();
        }
    }
}
