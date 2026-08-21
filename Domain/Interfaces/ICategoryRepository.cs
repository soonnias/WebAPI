using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories(string search, string sort);
        Task<Category> GetCategoryById(string id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(string id);
    }
   
}
