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
    public class OrderManagementService : IOrderManagementService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderManagementService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _orderRepository.GetOrdersByStatusAsync(status);
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task UpdateOrderStatusAsync(string orderId, string status)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, status);
        }
    }
}
