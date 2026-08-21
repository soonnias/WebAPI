namespace Infrastructure.Models
{
    public class Cart
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } // Ідентифікатор користувача
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Навігаційні властивості
        public virtual User User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
