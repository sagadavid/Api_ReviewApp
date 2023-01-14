using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public bool CreateCountry(Country country)
        {
            _datacontext.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _datacontext.Remove(country);
            return Save();
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

        public bool Save()
        {
            var saved = _datacontext.SaveChanges();
            return saved > 0;
        }

        public bool UpdateCountry(Country country)
        {
            _datacontext.Update(country);
            return Save();
        }
    }
}
