using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsByProductIdAsync(string productId);
        Task<Review> CreateReviewAsync(Review review);
        Task DeleteReviewAsync(string reviewId);
    }
}
