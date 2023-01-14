namespace Api_ReviewApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; } 
        public int Rating { get; set; }
        
        //relationships.. has one
        public Commentator Commentator { get; set; }
        public Pokemon Pokemon { get; set; }

    }
}
