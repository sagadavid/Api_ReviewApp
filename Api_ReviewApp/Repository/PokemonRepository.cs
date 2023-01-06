using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;

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

        public ICollection<Pokemon> GetPokemons()
        {
            return _datacontext.Pokemons.OrderBy(p => p.Id).ToList();
        }

    }
}
