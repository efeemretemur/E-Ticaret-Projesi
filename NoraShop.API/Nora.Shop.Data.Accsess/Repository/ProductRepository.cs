using System.Collections.Generic;
using System.Linq;
using Nora.Shop.Core.Entities;
using Nora.Shop.Core.Interfaces;
using Nora.Shop.DataAccess.Context;

namespace Nora.Shop.DataAccess.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly NoraShopContext _context;

        public ProductRepository(NoraShopContext context) : base(context)
        {
            _context = context;
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }

        public List<Product> SearchProducts(string keyword)
        {
            return _context.Products.Where(p => p.Name.Contains(keyword)).ToList();
        }
    }
}