using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductSizeRepository
    {
        Task<ProductSize> GetProductSizeByIdAsync(string productId, string sizeId);
        Task<ProductSize> GetProductSizeByIdJustAsync(string id);
        Task UpdateProductSizeAvailabilityAsync(string productId, string sizeId, bool isAvailable);
    }
}
