using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderManagementService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(string orderId);
        Task UpdateOrderStatusAsync(string orderId, string status);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
    }
}
