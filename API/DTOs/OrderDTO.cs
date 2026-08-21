namespace API.DTOs
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
    }
}
