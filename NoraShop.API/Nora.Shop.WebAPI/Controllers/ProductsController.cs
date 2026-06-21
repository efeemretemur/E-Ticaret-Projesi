using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Business.Services;
using Nora.Shop.Core.Entities;

namespace Nora.Shop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_productService.GetAllProducts());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetProductById(id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Product product)
        {
            _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Route id ve product id ayni olmali.");
            }

            var existingProduct = _productService.GetProductById(id);
            if (existingProduct is null)
            {
                return NotFound();
            }

            _productService.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var existingProduct = _productService.GetProductById(id);
            if (existingProduct is null)
            {
                return NotFound();
            }

            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
