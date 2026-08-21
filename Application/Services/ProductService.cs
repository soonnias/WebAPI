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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string searchString, List<string> categoryIds, string sortOrder)
        {
            return await _productRepository.GetProductsAsync(searchString, categoryIds, sortOrder);
        }

        public async Task<List<ProductSize>> GetSizesByProductIdAsync(string productId)
        {
            return await _productRepository.GetSizesByProductIdAsync(productId);
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
