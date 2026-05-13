using Nora.Shop.Core.Entities;
using Nora.Shop.Core.Interfaces;

namespace Nora.Shop.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetAllCategories() => _categoryRepository.GetAll();
        public Category GetCategoryById(int id) => _categoryRepository.GetById(id);
        public void AddCategory(Category category) => _categoryRepository.Add(category);
        public void UpdateCategory(Category category) => _categoryRepository.Update(category);
        public void DeleteCategory(int id) => _categoryRepository.Delete(id);
    }
}