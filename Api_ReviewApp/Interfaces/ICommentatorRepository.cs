using Api_ReviewApp.Models;

namespace Api_ReviewApp.Interfaces
{
    public interface ICommentatorRepository
    {
        ICollection<Commentator> GetCommentators();
        Commentator GetCommentator(int commentatorId);
        ICollection<Review> GetReviewsByCommentator(int commentatorId);
        bool CommentatorExists(int commentatorId);
        bool CreateCommentator(Commentator commentator);
        bool UpdateCommentator(Commentator commentator);
        bool DeleteCommentator(Commentator commentator);
        bool Save();
    }
}
