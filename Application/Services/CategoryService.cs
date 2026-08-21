using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategories(string search, string sort)
        {
            return await _categoryRepository.GetAllCategories(search, sort);
        }

        public async Task<Category> GetCategoryById(string id)
        {
            return await _categoryRepository.GetCategoryById(id);
        }

        public async Task<Category> AddCategory(Category category)
        {
            // Тут можна додати валідацію або бізнес-логіку, наприклад перевірку, чи вже існує така категорія
            if (category == null) throw new ArgumentNullException(nameof(category));
            await _categoryRepository.AddCategory(category);
            return category;
        }

        public async Task UpdateCategory(Category category)
        {
            // Можна додати додаткову перевірку
            if (category == null) throw new ArgumentNullException(nameof(category));
            await _categoryRepository.UpdateCategory(category);
        }

        public async Task DeleteCategory(string id)
        {
            // Можна додати перевірку перед видаленням
            await _categoryRepository.DeleteCategory(id);
        }
    }
}
