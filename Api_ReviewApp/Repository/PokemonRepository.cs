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

     
    }
}
