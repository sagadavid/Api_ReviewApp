namespace Api_ReviewApp.Models
{
    public class Commentator
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //relationships.. has many
        public ICollection<Review> Reviews { get; set;}
    }
}
