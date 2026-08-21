using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductSizeService
    {
        Task<ProductSize> GetProductSizeByIdJustAsync(string id);
        Task<ProductSize> GetProductSizeByIdAsync(string productId, string sizeId);
        Task UpdateProductSizeAvailabilityAsync(string productId, string sizeId, bool isAvailable);
    }
}
