using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(string searchString, List<string> categoryId, string sortOrder);
        Task AddProductAsync(Product product);
        Task<Product> GetProductByIdAsync(string id);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string id);
        Task<List<ProductSize>> GetSizesByProductIdAsync(string productId);
    }
}
