using System;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Review
    {
        private int _rating;
        private string _comment;

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string ProductId { get; set; }

        public int Rating
        {
            get => _rating;
            set
            {
                if (value < 1 || value > 5)
                    throw new InvalidOperationException("Rating must be between 1 and 5.");
                _rating = value;
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                if (value.Length > 200)
                    throw new InvalidOperationException("Comment cannot exceed 200 characters.");
                _comment = value;
            }
        }

        public DateTime CreatedAt { get; set; }

        // Конструктор для автоматичної ініціалізації дати створення
        public Review()
        {
            CreatedAt = DateTime.UtcNow;
        }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
