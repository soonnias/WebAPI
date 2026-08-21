using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductSizeRepository : IProductSizeRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductSizeRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.Models.ProductSize> GetProductSizeByIdAsync(string productId, string sizeId)
        {
            var productSizeEntity = await _context.ProductSizes
                .Include(ps => ps.Product)
                .Include(ps => ps.Size)
                .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.SizeId == sizeId);

            return _mapper.Map<Domain.Models.ProductSize>(productSizeEntity); // Маппінг з Infrastructure на Domain
        }

        public async Task<Domain.Models.ProductSize> GetProductSizeByIdJustAsync(string id)
        {
            var productSizeEntity = await _context.ProductSizes
                .Include(ps => ps.Product)
                .Include(ps => ps.Size)
                .FirstOrDefaultAsync(ps => ps.Id == id);

            return _mapper.Map<Domain.Models.ProductSize>(productSizeEntity); // Маппінг з Infrastructure на Domain
        }

        public async Task UpdateProductSizeAvailabilityAsync(string productId, string sizeId, bool isAvailable)
        {
            var productSizeEntity = await _context.ProductSizes
                .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.SizeId == sizeId);

            if (productSizeEntity != null)
            {
                productSizeEntity.IsAvailable = isAvailable;
                await _context.SaveChangesAsync();
            }
        }

    }
}
