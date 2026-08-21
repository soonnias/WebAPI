using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISizeService
    {
        Task<IEnumerable<SizeType>> GetAllSizeTypesAsync();
        Task<SizeType> GetSizeTypeByIdAsync(string id);
        Task AddSizeTypeAsync(SizeType sizeType);
        Task UpdateSizeTypeAsync(SizeType sizeType);
        Task DeleteSizeTypeAsync(string id);

        Task AddSizeAsync(Size size);
        Task<Size> GetSizeByIdAsync(string id);
        Task UpdateSizeAsync(Size size);
        Task DeleteSizeAsync(string id);

        Task<IEnumerable<Size>> GetSizesBySizeTypeIdAsync(string sizeTypeId);
    }
}
