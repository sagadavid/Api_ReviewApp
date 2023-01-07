using Api_ReviewApp.DTO;
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
            CreateMap<Pokemon, PokemonDto>();
        }
    }
}
