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
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Domain.Models.Order>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Маппінг зі сутностей в Domain
            return _mapper.Map<IEnumerable<Domain.Models.Order>>(orders);
        }

        public async Task<IEnumerable<Domain.Models.Order>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Маппінг зі сутностей в Domain
            return _mapper.Map<IEnumerable<Domain.Models.Order>>(orders);
        }

        public async Task<Domain.Models.Order> GetOrderByIdAsync(string orderId)
        {
            var orderEntity = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            // Маппінг зі сутності в Domain
            return _mapper.Map<Domain.Models.Order>(orderEntity);
        }

        public async Task UpdateOrderStatusAsync(string orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveOrderAsync(Domain.Models.Order order)
        {
            var orderEntity = _mapper.Map<Infrastructure.Models.Order>(order);  // Маппінг з Domain на Infrastructure
            _context.Orders.Add(orderEntity);
            await _context.SaveChangesAsync();
        }
    }
}
