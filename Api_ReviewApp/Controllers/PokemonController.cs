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
        private readonly IReviewRepository _reviewRepository;

        //automapper installed and injected, now controller using mapping ability
        public PokemonController(
            IPokemonRepository pokemonRepository, 
            IMapper mapper,
            IReviewRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
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

        [HttpGet("{pokemonId}")]
        [ProducesResponseType(200, Type=typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokemonId) 
        {
        if (!_pokemonRepository.PokemonExists(pokemonId)) { return NotFound(); }
        var pokemon = _mapper.Map<PokemonDto>
                (_pokemonRepository.GetPokemonById(pokemonId));
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
        return Ok(pokemon);
        }

        [HttpGet("{pokemonId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId)) { return NotFound(); }
            var rating = _pokemonRepository.GetPokemonRating(pokemonId);
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

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokemonId,
           [FromQuery] int ownerId, 
           [FromQuery] int categoryId,
           [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokemonId != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

            if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "feil med updating pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var reviewsToDelete = _reviewRepository.GetReviewsByPokemonId(pokemonId);
            var pokemonToDelete = _pokemonRepository.GetPokemonById(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }

    }
}
