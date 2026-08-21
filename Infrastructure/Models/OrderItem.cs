namespace Infrastructure.Models
{
    public class OrderItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductId { get; set; } // Зберігаємо ProductId
        public string ProductName { get; set; } // Назва продукту
        public decimal ProductPrice { get; set; } // Ціна продукту
        public int Quantity { get; set; } // Кількість товару
        public string Size { get; set; } // Розмір товару
        public string ImageUrl { get; set; } // URL зображення продукту
        public decimal TotalPrice { get; set; }
        public virtual Order Order { get; set; }
    }

}
