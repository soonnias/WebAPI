using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{ 
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task UpdateOrderStatusAsync(string orderId, string status);
        Task SaveOrderAsync(Order order);

    }
}
