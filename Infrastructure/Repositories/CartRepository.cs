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
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CartRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.Models.Cart> GetCartByUserIdAsync(string userId)
        {
            var cartEntity = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.ProductSize)
                         .ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Маппінг з Infrastructure.Cart на Domain.Cart
            var cart = _mapper.Map<Domain.Models.Cart>(cartEntity);

            return cart;
        }

        public async Task AddCartAsync(Domain.Models.Cart cart)
        {
            var cartEntity = _mapper.Map<Infrastructure.Models.Cart>(cart);  // Маппінг з Domain на Infrastructure
            _context.Carts.Add(cartEntity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartAsync(Domain.Models.Cart cart)
        {
            var cartEntity = _mapper.Map<Infrastructure.Models.Cart>(cart);  // Маппінг з Domain на Infrastructure
            _context.Carts.Remove(cartEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Domain.Models.Cart cart)
        {
            var existingCartEntity = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            if (existingCartEntity != null)
            {
                // Маппінг змін на існуючий Cart
                _mapper.Map(cart, existingCartEntity);

                // Збереження змін
                await _context.SaveChangesAsync();
            }
        }
    }
}
