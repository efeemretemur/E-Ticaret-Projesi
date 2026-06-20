using System.Collections.Generic;
using Nora.Shop.Core.Entities;

namespace Nora.Shop.Business.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetByCategory(int categoryId);
        List<Product> SearchProducts(string keyword);
    }
}