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

using AutoMapper;


namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Domain.Models.Category>> GetAllCategories(string search, string sort)
        {
            var categoriesQuery = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                categoriesQuery = categoriesQuery.Where(c => c.Name.Contains(search));
            }

            categoriesQuery = sort == "desc" ? categoriesQuery.OrderByDescending(c => c.Name) : categoriesQuery.OrderBy(c => c.Name);

            var categories = await categoriesQuery.ToListAsync();
            return _mapper.Map<IEnumerable<Domain.Models.Category>>(categories);
        }

        public async Task<Domain.Models.Category> GetCategoryById(string id)
        {
            var category = await _context.Categories
                .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<Domain.Models.Category>(category);
        }

        public async Task AddCategory(Domain.Models.Category category)
        {
            var categoryEntity = _mapper.Map<Infrastructure.Models.Category>(category);
            _context.Categories.Add(categoryEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(Domain.Models.Category category)
        {
            var categoryEntity = await _context.Categories.FindAsync(category.Id);

            if (categoryEntity == null)
            {
                throw new ArgumentException("Category not found.");
            }

            categoryEntity.Name = category.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
