using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }

        private string _status;

        public string Status
        {
            get => _status;
            set
            {
                if (!OrderStatus.AllowedStatuses.Contains(value))
                    throw new InvalidOperationException($"Invalid status: {value}. Allowed statuses are: {string.Join(", ", OrderStatus.AllowedStatuses)}");
                _status = value;
            }
        }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Бізнес-логіка: розрахунок загальної вартості
        public void CalculateTotalPrice()
        {
            Price = OrderItems.Sum(item => item.TotalPrice);
        }

        // Бізнес-логіка: перевірка адреси
        public void ValidateAddress()
        {
            if (string.IsNullOrWhiteSpace(Address))
                throw new InvalidOperationException("Order must have a valid address.");
        }
    }

    public static class OrderStatus
    {
        public const string Pending = "Pending";      // Замовлення в обробці
        public const string Shipped = "Shipped";      // Замовлення відправлено
        public const string Delivered = "Delivered";  // Замовлення доставлено
        public const string Cancelled = "Cancelled";  // Замовлення скасовано

        // Список допустимих статусів
        public static readonly HashSet<string> AllowedStatuses = new()
        {
            Pending,
            Shipped,
            Delivered,
            Cancelled
        };
    }

}
