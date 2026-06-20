using E_Ticaret_Projesi.Models;
using E_Ticaret_Projesi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using System.Diagnostics;

namespace E_Ticaret_Projesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var model = new HomeIndexViewModel
            {
                FeaturedProducts = _productService.GetAllProducts()
                    .OrderByDescending(p => p.Id)
                    .Take(6)
                    .ToList(),
                Categories = _categoryService.GetAllCategories()
                    .Take(4)
                    .ToList()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
