using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_ReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _datacontext;

        public ReviewRepository(DataContext dataContext)
        {
            _datacontext = dataContext;
        }

        public bool CreateReview(Review review)
        {
            _datacontext.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _datacontext.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _datacontext.RemoveRange(reviews);
            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return _datacontext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _datacontext.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsByPokemonId(int pokeId)
        {
            return _datacontext.Reviews
                .Where(r=>r.Pokemon.Id==pokeId)
                .ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return _datacontext.Reviews.Any();
        }

        public bool Save()
        {
         var saved = _datacontext.SaveChanges();
            return saved>0;
        }

        public bool UpdateReview(Review review)
        {
            _datacontext.Update(review);
            return Save();
        }
    }
}
