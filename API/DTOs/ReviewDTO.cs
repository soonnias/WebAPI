using Microsoft.AspNetCore.Mvc;

namespace API.DTOs
{
    public class CreateReviewDto
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } 
    }

    public class RewiewDto {
        public string Username { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class ReviewAllInfoDto  
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string Username { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
