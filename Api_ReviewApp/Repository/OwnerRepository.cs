using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;

namespace Api_ReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _datacontext;

        public OwnerRepository(DataContext dataContext)
        {
            _datacontext = dataContext;
        }

        public Owner GetOwner(int ownerId)
        {
            return _datacontext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerByPokemonId(int pokeId)
        {
            //return _dataContext.PokemonOwners.Where(po=>po.OwnerId== pokeId).ToList();//NOPE
            //GO JOIN TABLE, WHERE RIGHT AND SELECT LEFT (TO GET ENTITYFRAMEWORK GET NAVIGATIN PROPERTY)
            return _datacontext.PokemonOwners
                .Where(p => p.Pokemon.Id == pokeId)
                .Select(o => o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _datacontext.Owners.ToList();     
        }

        public ICollection<Pokemon> GetPokemonByOwnerId(int ownerId)
        {
            //where right select left
            return _datacontext.PokemonOwners
                .Where(w=>w.Owner.Id==ownerId)
                .Select(s=>s.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _datacontext.Owners.Any(o=>o.Id==ownerId);
        }
    }
}
