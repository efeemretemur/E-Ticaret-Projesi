using E_Ticaret_Projesi.ViewModels;
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
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        private const long MaxImageSizeInBytes = 5 * 1024 * 1024;

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public AdminProductController(
            IProductService productService,
            ICategoryService categoryService,
            IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;
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

            return View(new AdminProductFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminProductFormViewModel model)
        {
            ValidateImageFile(model.ImageFile);
            if (!ModelState.IsValid)
            {
                LoadCategories();
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name.Trim(),
                Description = model.Description.Trim(),
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                ImageUrl = await ResolveImageUrlAsync(model.ImageFile, model.ImageUrl)
            };

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
            return View(MapToForm(product));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminProductFormViewModel model)
        {
            var existingProduct = _productService.GetProductById(model.Id);
            if (existingProduct is null)
            {
                TempData["Error"] = "Düzenlemek istediğiniz ürün bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            ValidateImageFile(model.ImageFile);
            if (!ModelState.IsValid)
            {
                LoadCategories();
                return View(model);
            }

            var newImageUrl = await ResolveImageUrlAsync(model.ImageFile, model.ImageUrl, existingProduct.ImageUrl);
            if (!string.Equals(existingProduct.ImageUrl, newImageUrl, StringComparison.OrdinalIgnoreCase))
            {
                DeleteUploadedImage(existingProduct.ImageUrl);
            }

            existingProduct.Name = model.Name.Trim();
            existingProduct.Description = model.Description.Trim();
            existingProduct.Price = model.Price;
            existingProduct.Stock = model.Stock;
            existingProduct.CategoryId = model.CategoryId;
            existingProduct.ImageUrl = newImageUrl;

            _productService.UpdateProduct(existingProduct);
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

            DeleteUploadedImage(product.ImageUrl);
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

        private static AdminProductFormViewModel MapToForm(Product product) =>
            new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                ExistingImageUrl = product.ImageUrl
            };

        private void ValidateImageFile(IFormFile? imageFile)
        {
            if (imageFile is null || imageFile.Length == 0)
            {
                return;
            }

            var extension = Path.GetExtension(imageFile.FileName);
            if (!AllowedImageExtensions.Contains(extension))
            {
                ModelState.AddModelError(nameof(AdminProductFormViewModel.ImageFile), "Sadece JPG, PNG veya WEBP görseller yükleyebilirsiniz.");
            }

            if (imageFile.Length > MaxImageSizeInBytes)
            {
                ModelState.AddModelError(nameof(AdminProductFormViewModel.ImageFile), "Görsel boyutu en fazla 5 MB olabilir.");
            }
        }

        private async Task<string?> ResolveImageUrlAsync(IFormFile? imageFile, string? imageUrl, string? existingImageUrl = null)
        {
            if (imageFile is not null && imageFile.Length > 0)
            {
                return await SaveImageAsync(imageFile);
            }

            var normalizedImageUrl = string.IsNullOrWhiteSpace(imageUrl)
                ? null
                : imageUrl.Trim();

            if (!string.IsNullOrWhiteSpace(normalizedImageUrl))
            {
                return normalizedImageUrl;
            }

            return existingImageUrl;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsDirectory = Path.Combine(_environment.WebRootPath, "images", "products", "uploads");
            Directory.CreateDirectory(uploadsDirectory);

            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadsDirectory, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await imageFile.CopyToAsync(stream);

            return $"/images/products/uploads/{fileName}";
        }

        private void DeleteUploadedImage(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl) ||
                !imageUrl.StartsWith("/images/products/uploads/", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var relativePath = imageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(_environment.WebRootPath, relativePath);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }
    }
}
