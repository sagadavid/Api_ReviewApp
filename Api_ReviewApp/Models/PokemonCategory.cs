namespace Api_ReviewApp.Models
{/*
  this is a join table and is a necessity to connect many to many
    relationship between pokemon vs category */
    public class PokemonCategory
    {
        //make a composite key of two, later in datacontext.cs with fluent api
        public int PokemonId { get; set; }
        public int CategoryId { get; set;}

        //relationships..has one
        public Pokemon Pokemon { get; set;} 
        public Category Category { get; set;}

    }
}
