using Nora.Shop.Core.Entities;
using Nora.Shop.Core.Interfaces;

namespace Nora.Shop.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProducts() => _productRepository.GetAll();
        public Product GetProductById(int id) => _productRepository.GetById(id);
        public void AddProduct(Product product) => _productRepository.Add(product);
        public void UpdateProduct(Product product) => _productRepository.Update(product);
        public void DeleteProduct(int id) => _productRepository.Delete(id);
        public List<Product> GetByCategory(int categoryId) => _productRepository.GetProductsByCategory(categoryId);
        public List<Product> SearchProducts(string keyword) => _productRepository.SearchProducts(keyword);
    }
}