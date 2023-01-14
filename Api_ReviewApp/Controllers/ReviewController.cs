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
        private readonly ICommentatorRepository _CommentatorRepository;

        public ReviewController(
            IReviewRepository reviewRepository, 
            IMapper mapper,
            ICommentatorRepository CommentatorRepository,
            IPokemonRepository pokemonRepository

            )
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _CommentatorRepository = CommentatorRepository;
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
        public IActionResult CreateReview(
            //get check migrationSnapShot or datacontext and see relations,
            //see what entities are necessary in create/post method
            [FromQuery] int pokemonId,
             [FromQuery] int CommentatorId,
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
            //crucial point is that.. -'cause review is in relation with pokemon and Commentator-
            //we need to take them into play
            //to do that, we need to reach these two repositories
            //to do that, we need to inject them into cunstroctur or 
            var mapNyReview = _mapper.Map<Review>(createNyReview);
            mapNyReview.Pokemon = _pokemonRepository.GetPokemonById(pokemonId);
            mapNyReview.Commentator = _CommentatorRepository.GetCommentator(CommentatorId);

            if (!_reviewRepository.CreateReview(mapNyReview))
            {
                ModelState.AddModelError("", "failed saving ny review");
                return StatusCode(500, ModelState);
            }
            return Ok("ny review created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", " went wrong deleting owner");
            }

            return NoContent();
        }

        
        [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewsByReviewer(int commentatorId)
        {
            if (!_CommentatorRepository.CommentatorExists(commentatorId)) 
                return NotFound();

            var reviewsToDelete = _CommentatorRepository
                .GetReviewsByCommentator(commentatorId).ToList();
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
