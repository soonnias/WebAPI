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
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;

        public SizeService(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        public async Task<IEnumerable<SizeType>> GetAllSizeTypesAsync()
        {
            return await _sizeRepository.GetAllSizeTypesAsync();
        }

        public async Task<SizeType> GetSizeTypeByIdAsync(string id)
        {
            return await _sizeRepository.GetSizeTypeByIdAsync(id);
        }

        public async Task<IEnumerable<Size>> GetSizesBySizeTypeIdAsync(string sizeTypeId)
        {
            return await _sizeRepository.GetSizesBySizeTypeIdAsync(sizeTypeId);
        }

        public async Task AddSizeTypeAsync(SizeType sizeType)
        {
            await _sizeRepository.AddSizeTypeAsync(sizeType);
        }

        public async Task UpdateSizeTypeAsync(SizeType sizeType)
        {
            await _sizeRepository.UpdateSizeTypeAsync(sizeType);
        }

        public async Task DeleteSizeTypeAsync(string id)
        {
            // Логіка для видалення пов'язаної інформації може залишитися тут, якщо потрібно.
            var sizeType = await _sizeRepository.GetSizeTypeByIdAsync(id);
            if (sizeType != null)
            {
                foreach (var size in sizeType.Sizes)
                {
                    await _sizeRepository.DeleteSizeAsync(size.Id);
                }

                await _sizeRepository.DeleteSizeTypeAsync(id);
            }
        }

        public async Task AddSizeAsync(Size size)
        {
            await _sizeRepository.AddSizeAsync(size);
        }

        public async Task<Size> GetSizeByIdAsync(string id)
        {
            return await _sizeRepository.GetSizeByIdAsync(id);
        }

        public async Task UpdateSizeAsync(Size size)
        {
            await _sizeRepository.UpdateSizeAsync(size);
        }

        public async Task DeleteSizeAsync(string id)
        {
            await _sizeRepository.DeleteSizeAsync(id);
        }
    }
}
