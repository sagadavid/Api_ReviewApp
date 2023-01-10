using Api_ReviewApp.Models;

namespace Api_ReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        /*An interface is a programming construct 
         * that specifies a set of related methods 
         * that a class must implement,
         * without specifying how that behavior is implemented.
         the behaviour is defined in the not-interface class*/
         ICollection<Pokemon> GetPokemons();
         Pokemon GetPokemonById (int id);
         Pokemon GetPokemonByName (string name);
         decimal GetPokemonRating (int pokeId);
         bool PokemonExists (int pokeId);

    }
}
