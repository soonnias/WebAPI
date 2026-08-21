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
    public class ProductSizeService : IProductSizeService
    {
        private readonly IProductSizeRepository _productSizeRepository;

        public ProductSizeService(IProductSizeRepository productSizeRepository)
        {
            _productSizeRepository = productSizeRepository;
        }

        public async Task<ProductSize> GetProductSizeByIdJustAsync(string id)
        {
            return await _productSizeRepository.GetProductSizeByIdJustAsync(id);
        }

        public async Task<ProductSize> GetProductSizeByIdAsync(string productId, string sizeId)
        {
            return await _productSizeRepository.GetProductSizeByIdAsync(productId, sizeId);
        }

        public async Task UpdateProductSizeAvailabilityAsync(string productId, string sizeId, bool isAvailable)
        {
            var productSize = await _productSizeRepository.GetProductSizeByIdAsync(productId, sizeId);
            if (productSize != null)
            {
                productSize.IsAvailable = isAvailable;

                // Тут залежно від імплементації Update можна зробити метод SaveChanges, якщо репозиторій реалізує Unit of Work
                await _productSizeRepository.UpdateProductSizeAvailabilityAsync(productId, sizeId, isAvailable);
            }
        }
    }
}
