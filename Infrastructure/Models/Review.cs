namespace Infrastructure.Models
{
    public class Review
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Rating { get; set; } // Оцінка (1-5)
        public string Comment { get; set; } // Коментар
        public DateTime CreatedAt { get; set; }
    }

}
