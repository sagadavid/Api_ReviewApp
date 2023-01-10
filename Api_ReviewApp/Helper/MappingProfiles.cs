using Api_ReviewApp.Dto;
using Api_ReviewApp.Models;
using AutoMapper;

namespace Api_ReviewApp.Helper
{
    //can inheret profile by automapper pack installed
    //automapper should be presented as service in program.cs
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //define source and destination map points before using imapper
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Reviewer, ReviewerDto>();
            

        }
    }
}
