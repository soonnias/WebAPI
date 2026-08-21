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

using AutoMapper;


namespace Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Отримати всі відгуки для певного продукту
        public async Task<List<Domain.Models.Review>> GetReviewsByProductIdAsync(string productId)
        {
            var reviewEntities = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User) // Включаємо користувача, щоб можна було використовувати інформацію про нього
                .ToListAsync();

            return _mapper.Map<List<Domain.Models.Review>>(reviewEntities); // Маппінг з Infrastructure на Domain
        }

        // Створення нового відгуку
        public async Task<Domain.Models.Review> CreateReviewAsync(Domain.Models.Review review)
        {
            var reviewEntity = _mapper.Map<Infrastructure.Models.Review>(review); // Маппінг з Domain на Infrastructure
            _context.Reviews.Add(reviewEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<Domain.Models.Review>(reviewEntity); // Маппінг назад на Domain
        }

        // Видалення відгуку
        public async Task DeleteReviewAsync(string reviewId)
        {
            var reviewEntity = await _context.Reviews.FindAsync(reviewId);
            if (reviewEntity != null)
            {
                _context.Reviews.Remove(reviewEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
