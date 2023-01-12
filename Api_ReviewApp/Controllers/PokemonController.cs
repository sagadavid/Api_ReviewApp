using Api_ReviewApp.Dto;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using Api_ReviewApp.Repository;
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
            var pokemons = _mapper.Map<List<PokemonDto>>
                (_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type=typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId) 
        {
        if (!_pokemonRepository.PokemonExists(pokeId)) { return NotFound(); }
        var pokemon = _mapper.Map<PokemonDto>
                (_pokemonRepository.GetPokemonById(pokeId));
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
        return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId)) { return NotFound(); }
            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon(
            [FromQuery] int ownerId,
             [FromQuery] int categoryId,
            [FromBody] PokemonDto createNyPokemon)
        {
            //means, if no pokemon input in post method
            if (createNyPokemon == null) return BadRequest();

            var pokemon = _pokemonRepository.GetPokemons()
                    .Where(c => c.Name.Trim().ToUpper()
                    == createNyPokemon.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "already exists..not a new pokemon");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);


            var mapNyPokemon = _mapper.Map<Pokemon>(createNyPokemon);

            if (!_pokemonRepository.CreatePokemon(mapNyPokemon,ownerId,categoryId))
            {
                ModelState.AddModelError("", "failed saving ny pokemon");
                return StatusCode(500, ModelState);
            }
            return Ok("ny pokemon created");
        }


    }
}
