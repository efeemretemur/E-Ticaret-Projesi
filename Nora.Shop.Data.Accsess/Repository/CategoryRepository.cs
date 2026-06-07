using Microsoft.EntityFrameworkCore;
using Nora.Shop.Core.Entities;
using Nora.Shop.Core.Interfaces;
using Nora.Shop.DataAccess.Context;

namespace Nora.Shop.DataAccess.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly NoraShopContext _context;

        public CategoryRepository(NoraShopContext context) : base(context)
        {
            _context = context;
        }

        public List<Category> GetCategoriesWithProducts()
        {
            return _context.Categories
                .Include(category => category.Products)
                .OrderBy(category => category.Name)
                .ToList();
        }
    }
}
