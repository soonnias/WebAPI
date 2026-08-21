using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<List<Review>> GetReviewsByProductIdAsync(string productId)
        {
            return await _reviewRepository.GetReviewsByProductIdAsync(productId);
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            return await _reviewRepository.CreateReviewAsync(review);
        }

        public async Task DeleteReviewAsync(string reviewId)
        {
            await _reviewRepository.DeleteReviewAsync(reviewId);
        }
    }
}
