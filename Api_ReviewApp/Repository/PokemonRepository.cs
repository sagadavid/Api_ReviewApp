using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_ReviewApp.Repository
{
    //a repository is where you make database calls for the sake of abstraction
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _datacontext;
        public PokemonRepository(DataContext dataContext)
        {
            _datacontext = dataContext;
        }

        public bool CreatePokemon(Pokemon pokemon, int ownerId, int categoryId)
        {
            //for den pokemonen er knyttet til disse to, trenger a trekke dem,
            //saledes blir det mulig a lage/skape en pokemon (ved a lage disse dataene)
            var pokemonOwnerPerSe = _datacontext.Owners
                .Where(a=>a.Id==ownerId).FirstOrDefault();
            var categoryPerSe = _datacontext.Categories
                .Where(c=>c.Id==categoryId).FirstOrDefault();

            //instead of using auto mapper(to be sure or clear),
            //hardcoding to create pokemon owner
            //notice syntax here!
            var nyPokemonOwner = new PokemonOwner()
            {
                //pokemonowner.owner ! peek definition
                Owner = pokemonOwnerPerSe,
                Pokemon = pokemon,
            };

            var nyPokemonCategory = new PokemonCategory()
            {
                //pokemoncategory.category ! peek definition
                Category = categoryPerSe,
                Pokemon = pokemon,
            };

            _datacontext.Add(nyPokemonOwner);
            _datacontext.Add(nyPokemonCategory);
            _datacontext.Add(pokemon);

            return Save();



        }

        public Pokemon GetPokemonById (int id)
        {
            return _datacontext.Pokemon
                .Where(p=>p.Id==id).FirstOrDefault();
        }

        public Pokemon GetPokemonByName (string name)
        {
            return _datacontext.Pokemon
                .Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating (int pokeId)
        {
            //there are many ratings in many reviews
            var reviews = _datacontext.Reviews
                .Where(p=>p.Pokemon.Id== pokeId);

            //a rating is average of ratings
            if (!reviews.Any()) return 0;

            //notice return time is a decimal
            return ((decimal)reviews
                .Sum(r=>r.Rating)/reviews.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _datacontext.Pokemon
                .OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            //any returns boolean
            return _datacontext.Pokemon
                .Any(p => p.Id == pokeId);
        }

        public bool Save()
        {
            var saved = _datacontext.SaveChanges();
            return saved > 0;
        }
    }
}
