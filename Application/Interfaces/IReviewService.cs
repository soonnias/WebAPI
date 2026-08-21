using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsByProductIdAsync(string productId);
        Task<Review> CreateReviewAsync(Review review);
        Task DeleteReviewAsync(string reviewId);
    }
}
