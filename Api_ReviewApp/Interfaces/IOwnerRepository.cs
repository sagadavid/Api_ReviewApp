using Api_ReviewApp.Models;

namespace Api_ReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnerByPokemonId(int pokeId);
        ICollection<Pokemon> GetPokemonByOwnerId(int ownerId);
        bool OwnerExists(int ownerId);
        
    }
}
