using System.Collections.Generic;
using Nora.Shop.Core.Entities;

namespace Nora.Shop.Core.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        List<Category> GetCategoriesWithProducts();
    }
}