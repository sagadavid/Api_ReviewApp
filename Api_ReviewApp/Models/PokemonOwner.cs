namespace Api_ReviewApp.Models
{
    /*
  this is a join table and is a necessity to connect many to many
    relationship between pokemon vs owner */
    public class PokemonOwner
    {
        public int PokemonId { get; set; }
        public int OwnerId { get; set; }

        //relationships.. has one
        public Pokemon Pokemon { get; set; }
        public Owner Owner { get; set; }
    }
}
