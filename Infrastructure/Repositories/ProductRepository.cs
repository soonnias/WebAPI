using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;



namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Domain.Models.Product>> GetProductsAsync(string searchString, List<string> categoryIds, string sortOrder)
        {
            var query = _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Include(p => p.Reviews)
                .ThenInclude(pc=> pc.User)
                .Include(p => p.ProductSizes)
                .ThenInclude(ps => ps.Size)
                .AsQueryable();

            if (categoryIds != null && categoryIds.Any())
            {
                query = query.Where(p => p.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId)));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Name.Contains(searchString));
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(p => p.Name),
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name),
            };

            var products = await query.ToListAsync();
            return _mapper.Map<IEnumerable<Domain.Models.Product>>(products); // Маппінг з Infrastructure на Domain
        }

        public async Task<List<Domain.Models.ProductSize>> GetSizesByProductIdAsync(string productId)
        {
            var sizes = await _context.ProductSizes
                .Include(ps => ps.Size)

                .Where(ps => ps.ProductId == productId)
                .ToListAsync();

            return _mapper.Map<List<Domain.Models.ProductSize>>(sizes); // Маппінг з Infrastructure на Domain
        }

        public async Task<Domain.Models.Product> GetProductByIdAsync(string id)
        {
            var product = await _context.Products
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .Include(p => p.Reviews)
                .ThenInclude(pc => pc.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<Domain.Models.Product>(product); // Маппінг з Infrastructure на Domain
        }

        public async Task AddProductAsync(Domain.Models.Product product)
        {
            var productEntity = _mapper.Map<Infrastructure.Models.Product>(product); // Маппінг з Domain на Infrastructure
            _context.Products.Add(productEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Domain.Models.Product product)
        {
            var existingProduct = await _context.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.ImageUrl = product.ImageUrl;

                existingProduct.ProductCategories.Clear();
                existingProduct.ProductCategories = product.ProductCategories
                    .Select(pc => _mapper.Map<Infrastructure.Models.ProductCategory>(pc))
                    .ToList();

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
