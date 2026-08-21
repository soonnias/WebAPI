namespace Infrastructure.Models
{
    public class CartItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CartId { get; set; } // Зовнішній ключ на Cart
        public string ProductId { get; set; } // Зовнішній ключ на Product
        public string ProductSizeId { get; set; } // Зовнішній ключ на Size
        public int Quantity { get; set; } // Кількість товару
        public decimal Price { get; set; } // Сума товару
        public string UserId { get; set; }


        // Навігаційні властивості
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; } // Припускаємо, що у вас є клас Product
        public virtual ProductSize ProductSize { get; set; } // Зв'язок з ProductSize
    }

}
