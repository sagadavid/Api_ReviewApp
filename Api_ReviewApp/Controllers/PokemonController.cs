using Api_ReviewApp.DTO;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api_ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        //automapper installed and injected, now controller using mapping ability
        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }
        
        //note that methods used here are defined in repository file

        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemons()
        {
            //automapper used to get list call
            var pokemons = _mapper
                .Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type=typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId) 
        {
        if (!_pokemonRepository.PokemonExists(pokeId)) { return NotFound(); }
        var pokemon = _mapper
                .Map<PokemonDto>(_pokemonRepository.GetPokemonById(pokeId));
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
        return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId)) { return NotFound(); }
            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            return Ok(rating);
        }


    }
}
