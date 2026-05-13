using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using Nora.Shop.Core.Entities;

namespace E_Ticaret_Projesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public AdminCategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View(_categoryService.GetAllCategories());
        }

        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _categoryService.AddCategory(category);
            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category is null)
            {
                TempData["Error"] = "Düzenlemek istediğiniz kategori bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _categoryService.UpdateCategory(category);
            TempData["Success"] = "Kategori güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (_productService.GetByCategory(id).Any())
            {
                TempData["Error"] = "Bu kategoriye ait ürünler olduğu için önce ürünleri taşıyın veya silin.";
                return RedirectToAction(nameof(Index));
            }

            _categoryService.DeleteCategory(id);
            TempData["Success"] = "Kategori silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
