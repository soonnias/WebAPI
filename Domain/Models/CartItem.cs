using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CartItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CartId { get; set; }
        public string ProductId { get; set; }
        public string ProductSizeId { get; set; }
        public string UserId { get; set; }

        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new InvalidOperationException("Quantity cannot be negative.");
                _quantity = value;
            }
        }

        public decimal Price { get; set; }

        // Бізнес-логіка: метод для збільшення кількості
        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Increase amount must be positive.");
            Quantity += amount;
        }

        // Бізнес-логіка: метод для зменшення кількості
        public void DecreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Decrease amount must be positive.");
            if (Quantity - amount < 0)
                throw new InvalidOperationException("Quantity cannot be less than zero.");
            Quantity -= amount;
        }


        // Навігаційні властивості
        [JsonIgnore]
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductSize ProductSize { get; set; }

    }

}
