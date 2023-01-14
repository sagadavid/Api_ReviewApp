using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore;

namespace Api_ReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _datacontext;

        public OwnerRepository(DataContext dataContext)
        {
            _datacontext = dataContext;
        }

        public bool CreateOwner(Owner owner)
        {
            _datacontext.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _datacontext.Remove(owner);
            return Save();
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

        public bool Save()
        {
            var saved =_datacontext.SaveChanges();
            //error#03 above: 
            //SqlException: The INSERT statement conflicted with the FOREIGN KEY constraint
            //"FK_Owners_Countries_CountryId". The conflict occurred in database "reviewappdb",
            //table "dbo.Countries", column 'Id'.The statement has been terminated.
            
            //solution#03:'cause there is fk issue go check data context model snapshot..
            //there is obvious that an owner cant exist/and tehrefore created without a countryId
            //so.. the solution idea is to carry along country id into creation process of owner.
            //to get that data in.. follow steps with this code solution#03

            return saved >0;
        }

        public bool UpdateOwner(Owner owner)
        {
            _datacontext.Update(owner);
            return Save(); ;
        }
    }
}
