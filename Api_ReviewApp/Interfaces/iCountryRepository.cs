using Api_ReviewApp.Models;

namespace Api_ReviewApp.Interfaces
{
    //inject interfaces to program.cs
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountryById(int countryId);
        Country GetCountryByOwnerId(int ownerId);
        ICollection<Owner> GetOwnersByCountryId(int countryId);
        bool CountryExists(int countryId);
        bool CreateCountry(Country country);
        bool Save();
    }
}
