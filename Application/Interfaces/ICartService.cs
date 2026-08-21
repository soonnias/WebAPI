using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<CartItem> AddToCartAsync(string productId, string selectedSizeId, int quantity, string userId);
        Task UpdateCartItemAsync(string cartItemId, int quantity, decimal price);
        Task RemoveFromCartAsync(string cartItemId);
        Task ClearCartAsync(string userId);
    }
}
