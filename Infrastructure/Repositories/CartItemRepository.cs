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
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CartItemRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.Models.CartItem> GetCartItemAsync(string cartId, string productId, string productSizeId, string userId)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product) // Завантажити Product
                .ThenInclude(p => p.ProductSizes) // Завантажити ProductSizes
                .Include(ci => ci.ProductSize) // Завантажити ProductSize
                .ThenInclude(ps => ps.Size) // Завантажити Size
                .FirstOrDefaultAsync(ci => ci.CartId == cartId &&
                                           ci.ProductId == productId &&
                                           ci.ProductSizeId == productSizeId &&
                                           ci.UserId == userId);

            return _mapper.Map<Domain.Models.CartItem>(cartItem);  // Маппінг на Domain модель
        }


        public async Task<Domain.Models.CartItem> GetCartItemByIdAsync(string cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            return _mapper.Map<Domain.Models.CartItem>(cartItem);  // Маппінг на Domain модель
        }

        public async Task AddCartItemAsync(Domain.Models.CartItem cartItem)
        {
            var cartItemEntity = _mapper.Map<Infrastructure.Models.CartItem>(cartItem); // Маппінг з Domain на Infrastructure
            _context.CartItems.Add(cartItemEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(Domain.Models.CartItem cartItem)
        {
            //var cartItemEntity = _mapper.Map<Infrastructure.Models.CartItem>(cartItem); // Маппінг з Domain на Infrastructure
            //_context.CartItems.Update(cartItemEntity);
            //await _context.SaveChangesAsync();

            // Перевірити, чи об'єкт уже відслідковується
            var trackedEntity = await _context.CartItems.FindAsync(cartItem.Id);

            if (trackedEntity != null)
            {
                // Оновлення значень у відслідковуваному об'єкті
                trackedEntity.Quantity = cartItem.Quantity;
                trackedEntity.Price = cartItem.Price;
            }
            else
            {
                // Якщо об'єкт не відслідковується, додаємо його вручну
                var cartItemEntity = _mapper.Map<Infrastructure.Models.CartItem>(cartItem);
                _context.CartItems.Attach(cartItemEntity);
                _context.Entry(cartItemEntity).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }


        public async Task RemoveCartItemAsync(Domain.Models.CartItem cartItem)
        {
            // Знайти об'єкт у базі даних
            var cartItemEntity = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItem.Id);

            if (cartItemEntity == null)
            {
                throw new ArgumentException($"CartItem with ID {cartItem.Id} not found.");
            }

            // Видалити знайдений об'єкт
            _context.CartItems.Remove(cartItemEntity);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveRangeCartItemsAsync(IEnumerable<Domain.Models.CartItem> cartItems)
        {
            var cartItemsEntities = _mapper.Map<IEnumerable<Infrastructure.Models.CartItem>>(cartItems); // Маппінг з Domain на Infrastructure
            _context.CartItems.RemoveRange(cartItemsEntities);
            await _context.SaveChangesAsync();
        }
    }
}
