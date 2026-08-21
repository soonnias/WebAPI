using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Models;
using Domain.Interfaces;
using AutoMapper;

namespace Infrastructure.Repositories
{
    public class SizeRepository : ISizeRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SizeRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SizeType>> GetAllSizeTypesAsync()
        {
            var sizeTypes = await _context.SizeTypes
                .Include(st => st.Sizes) // Включаємо зв'язок з розмірами
                .ToListAsync();

            return _mapper.Map<IEnumerable<SizeType>>(sizeTypes);
        }

        public async Task<SizeType> GetSizeTypeByIdAsync(string id)
        {
            var sizeType = await _context.SizeTypes
                .Include(st => st.Sizes)
                .FirstOrDefaultAsync(st => st.Id == id);

            return _mapper.Map<SizeType>(sizeType);
        }

        public async Task<IEnumerable<Size>> GetSizesBySizeTypeIdAsync(string sizeTypeId)
        {
            var sizes = await _context.Sizes
                .Where(s => s.SizeTypeId == sizeTypeId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<Size>>(sizes);
        }

        public async Task AddSizeTypeAsync(SizeType sizeType)
        {
            var sizeTypeEntity = _mapper.Map<Infrastructure.Models.SizeType>(sizeType);
            _context.SizeTypes.Add(sizeTypeEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSizeTypeAsync(SizeType sizeType)
        {
            var sizeTypeEntity = _mapper.Map<Infrastructure.Models.SizeType>(sizeType);
            _context.SizeTypes.Update(sizeTypeEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSizeTypeAsync(string id)
        {
            var sizeType = await _context.SizeTypes.FindAsync(id);
            if (sizeType != null)
            {
                _context.SizeTypes.Remove(sizeType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddSizeAsync(Size size)
        {
            var sizeEntity = _mapper.Map<Infrastructure.Models.Size>(size);
            _context.Sizes.Add(sizeEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Size> GetSizeByIdAsync(string id)
        {
            var size = await _context.Sizes.FindAsync(id);
            return _mapper.Map<Size>(size);
        }

        public async Task UpdateSizeAsync(Size size)
        {
            var sizeEntity = _mapper.Map<Infrastructure.Models.Size>(size);
            _context.Sizes.Update(sizeEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSizeAsync(string id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size != null)
            {
                _context.Sizes.Remove(size);
                await _context.SaveChangesAsync();
            }
        }
    }
}
