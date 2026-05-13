using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using Nora.Shop.Core.Entities;

namespace E_Ticaret_Projesi.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index(int? categoryId, string? search)
        {
            IEnumerable<Product> products = _productService.GetAllProducts();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var keyword = search.Trim();
                products = products.Where(p =>
                    p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.Categories = _categoryService.GetAllCategories();
            ViewBag.SelectedCategory = categoryId;
            ViewBag.Search = search;

            return View(products.ToList());
        }

        public IActionResult Detail(int id)
        {
            var product = _productService.GetProductById(id);
            if (product is null)
            {
                TempData["Error"] = "İstediğiniz ürün bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
    }
}
