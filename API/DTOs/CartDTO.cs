namespace API.DTOs
{
    public class AddToCartDto
    {
        public string ProductId { get; set; }
        public string SelectedSizeId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateCartItemDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class CartDto
    {
        public string CartId { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; } // Загальна сума кошика
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>(); // Елементи кошика
    }

    public class CartItemDto
    {
        public string CartItemId { get; set; } // ID товару в кошику
        public string ProductId { get; set; } // ID продукту
        public string ProductName { get; set; } // Назва продукту
        public string SelectedSize { get; set; } // Обраний розмір
        public string ProductSizeId { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Price { get; set; } // Ціна продукту
        public int Quantity { get; set; } // Кількість товару
        public decimal TotalItemPrice { get; set; } // Загальна вартість товару (Price * Quantity)
    }
}
