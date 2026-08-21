using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task<Order> CreateOrderAsync(User user, string address);
        Task<bool> SaveOrderAsync(Order order);
        Task<Order> FindOrderByIdAsync(string orderId);
        Task<bool> CancelOrderAsync(string orderId);
    }
}
