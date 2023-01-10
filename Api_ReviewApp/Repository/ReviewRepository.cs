using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;

namespace Api_ReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _dataContext;

        public ReviewRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public Review GetReview(int reviewId)
        {
            return _dataContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _dataContext.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsByPokemonId(int pokeId)
        {
            return _dataContext.Reviews
                .Where(r=>r.Pokemon.Id==pokeId)
                .ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return _dataContext.Reviews.Any();
        }
    }
}
