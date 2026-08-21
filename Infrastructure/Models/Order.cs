
namespace Infrastructure.Models
{

    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; } // Nullable UserId
        public virtual User? User { get; set; } // Nullable User reference
        public DateTime OrderDate { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public string Status { get; set; }
    }

}
