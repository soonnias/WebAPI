using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories(string search, string sort);
        Task<Category> GetCategoryById(string id);
        Task<Category> AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(string id);
    }
}
