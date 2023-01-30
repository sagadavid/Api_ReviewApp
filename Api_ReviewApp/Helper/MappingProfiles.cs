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
            //dto2category mapping is required for post/create methods

            //CreateMap<Pokemon, PokemonDto>();
            //CreateMap<PokemonDto, Pokemon>();
                    //reversemap does the same thing
             CreateMap<Pokemon, PokemonDto>().ReverseMap();
           
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
           
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();

            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();

            CreateMap<Commentator, CommentatorDto>();
            CreateMap<CommentatorDto, Commentator>();
            
        }
    }
}
