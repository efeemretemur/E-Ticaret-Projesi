using System.Collections.Generic;
using System.Linq;
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
            // Kategorileri altındaki ürünlerle birlikte (Include ederek) listeliyoruz
            return _context.Categories.Include(c => c.Products).ToList();
        }
    }
}