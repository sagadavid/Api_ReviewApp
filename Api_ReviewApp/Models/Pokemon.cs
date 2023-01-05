namespace Api_ReviewApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        // has many
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PokemonOwners> PokemonOwners { get; set; }
        public ICollection<PokemonCategories> PokemonCategories { get; set; }

    }
}
