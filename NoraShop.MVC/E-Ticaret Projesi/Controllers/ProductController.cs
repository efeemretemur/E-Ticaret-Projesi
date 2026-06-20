using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Core.Entities;
using System.Net.Http.Json;

namespace E_Ticaret_Projesi.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public ProductController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            var apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7170";
            _apiUrl = $"{apiBaseUrl.TrimEnd('/')}/api/products";
        }

        public async Task<IActionResult> Index(int? categoryId, string search)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Product>>(_apiUrl);
            IEnumerable<Product> products = response ?? new List<Product>();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                               p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.Categories = new List<Category>();
            ViewBag.SelectedCategory = categoryId;
            ViewBag.Search = search;

            return View(products.ToList());
        }

        public async Task<IActionResult> Detail(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "İstediğiniz ürün bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var product = await response.Content.ReadFromJsonAsync<Product>();
            return View(product);
        }
    }
}
