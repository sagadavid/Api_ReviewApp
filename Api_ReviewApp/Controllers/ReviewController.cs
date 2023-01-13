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
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        //see explanation**
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(
            IReviewRepository reviewRepository, 
            IMapper mapper,
            IReviewerRepository reviewerRepository,
            IPokemonRepository pokemonRepository

            )
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>
                (_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("reviews/{pokeId}")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByPokemonId(int pokeId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>
                (_reviewRepository.GetReviewsByPokemonId(pokeId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon(
            //get check migrationSnapShot or datacontext and see relations,
            //see what entities are necessary in create/post method
            [FromQuery] int pokemonId,
             [FromQuery] int reviewerId,
            [FromBody] ReviewDto createNyReview)
        {
            if (createNyReview == null) return BadRequest();

            var reviews = _reviewRepository.GetReviews()
                    .Where(c => c.Title.Trim().ToUpper() 
                    == createNyReview.Title.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviews != null)
            {
                ModelState.AddModelError("", "already exists..not a new review");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            //explanation**
            //crucial point is that.. -'cause review is in relation with pokemon and reviewer-
            //we need to take them into play
            //to do that, we need to reach these two repositories
            //to do that, we need to inject them into cunstroctur or 
            var mapNyReview = _mapper.Map<Review>(createNyReview);
            mapNyReview.Pokemon = _pokemonRepository.GetPokemonById(pokemonId);
            mapNyReview.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(mapNyReview))
            {
                ModelState.AddModelError("", "failed saving ny review");
                return StatusCode(500, ModelState);
            }
            return Ok("ny review created");
        }


    }
}
