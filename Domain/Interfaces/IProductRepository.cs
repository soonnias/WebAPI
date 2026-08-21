using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(string searchString, List<string> categoryIds, string sortOrder);
        Task<List<ProductSize>> GetSizesByProductIdAsync(string productId);
        Task<Product> GetProductByIdAsync(string id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string id);
    }
}
