using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using Nora.Shop.Core.Entities;

namespace Nora.Shop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public CategoriesController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_categoryService.GetAllCategories());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _categoryService.AddCategory(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("Route id ve category id ayni olmali.");
            }

            var existingCategory = _categoryService.GetCategoryById(id);
            if (existingCategory is null)
            {
                return NotFound();
            }

            _categoryService.UpdateCategory(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingCategory = _categoryService.GetCategoryById(id);
            if (existingCategory is null)
            {
                return NotFound();
            }

            if (_productService.GetByCategory(id).Any())
            {
                return Conflict("Bu kategoriye bagli urunler oldugu icin silinemez.");
            }

            _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
