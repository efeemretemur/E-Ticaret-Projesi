using Nora.Shop.Core.Entities;

namespace Nora.Shop.Core.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> GetProductsByCategory(int categoryId);
        List<Product> SearchProducts(string keyword);
    }
}