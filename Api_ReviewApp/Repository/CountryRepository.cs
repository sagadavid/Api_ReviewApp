using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using AutoMapper;
using System.Collections.Immutable;
using System.Linq;

namespace Api_ReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _datacontext;
        public CountryRepository(DataContext datacontext)
        {
            _datacontext= datacontext;
        }

        public bool CountryExists(int countryId)
        {
            return _datacontext
                .Countries.Any(c=>c.Id==countryId);
        }

        public ICollection<Country> GetCountries()
        {
            return _datacontext
                .Countries.ToArray();
        }

        public Country GetCountryById(int countryId)
        {
            return _datacontext.Countries
                .Where(c => c.Id == countryId).FirstOrDefault();
        }

        public Country GetCountryByOwnerId(int ownerId)
        {
            return _datacontext.Owners
                .Where(o => o.Id == ownerId)
                .Select(c => c.Country).FirstOrDefault();

        }

        public ICollection<Owner> GetOwnersByCountryId(int countryId)
        {
            return _datacontext
                .Owners
                .Where(c => c.Country.Id == countryId).ToList();
        }
    }
}
