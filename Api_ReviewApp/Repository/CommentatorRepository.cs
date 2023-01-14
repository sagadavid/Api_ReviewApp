using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using AutoMapper;

namespace Api_ReviewApp.Repository
{
    public class CommentatorRepository : ICommentatorRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CommentatorRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        //'cause there is no one to one relation here, 
        //we dont need to import any other entity as parameter
        public bool CreateCommentator(Commentator Commentator)
        {
            _dataContext.Add(Commentator);
            return Save();
        }

        public Commentator GetCommentator(int CommentatorId)
        {
            return _dataContext.Commentators
                .Where(r=>r.Id==CommentatorId).FirstOrDefault();
        }

        public ICollection<Commentator> GetCommentators()
        {
            return _dataContext.Commentators.ToList();
        }

        public ICollection<Review> GetReviewsByCommentator(int CommentatorId)
        {
            return _dataContext.Reviews
                .Where(r=>r.Commentator.Id==CommentatorId).ToList();
        }

        public bool CommentatorExists(int CommentatorId)
        {
            return _dataContext.Commentators.Any();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ;
        }

        public bool UpdateReviewer(Commentator commentator)
        {
            _dataContext.Update(commentator);
            return Save();
        }

        public bool DeleteReviewer(Commentator commentator)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCommentator(Commentator commentator)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCommentator(Commentator commentator)
        {
            _dataContext.Remove(commentator);
            return Save();
        }
    }
}
