namespace Api_ReviewApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //has many
        public ICollection<PokemonCategories> PokemonCategories { get; set;}
    }
}
