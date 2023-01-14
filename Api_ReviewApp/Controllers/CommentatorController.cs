using Api_ReviewApp.Dto;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using Api_ReviewApp.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Api_ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CommentatorController : Controller
    {
        private readonly ICommentatorRepository _commentatorRepository;
        private readonly IMapper _mapper;

        public CommentatorController(ICommentatorRepository CommentatorRepository, IMapper mapper)
        {
            _commentatorRepository = CommentatorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Commentator>))]
        public IActionResult GetCommentators()
        {
            var Commentators = _mapper.Map<List<CommentatorDto>>
                (_commentatorRepository.GetCommentators());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Commentators);
        }

        [HttpGet("{CommentatorId}")]
        [ProducesResponseType(200, Type = typeof(Commentator))]
        [ProducesResponseType(400)]
        public IActionResult GetCommentator(int CommentatorId)
        {
            if (!_commentatorRepository.CommentatorExists(CommentatorId))
                return NotFound();

            var Commentator = _mapper.Map<CommentatorDto>
                (_commentatorRepository.GetCommentator(CommentatorId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Commentator);
        }

        [HttpGet("{CommentatorId}/reviews")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviewsByCommentator(int CommentatorId)
        {
            if (!_commentatorRepository.CommentatorExists(CommentatorId))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewDto>>(
                _commentatorRepository.GetReviewsByCommentator(CommentatorId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCommentator([FromBody] CommentatorDto createCommentator)
        {
            if (createCommentator == null) return BadRequest(ModelState);
            var CommentatorMatch = _commentatorRepository.GetCommentators()
                .Where(r => r.LastName.Trim().ToUpper()
                == createCommentator.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (CommentatorMatch != null)
            {
                ModelState.AddModelError("", "already exists..not a new Commentator");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            //real creation is this.. mapping to Commentator file
            var mapCommentator = _mapper.Map<Commentator>(CommentatorMatch);

            if (!_commentatorRepository.CreateCommentator(mapCommentator))
            {
                ModelState.AddModelError("", "failed saving Commentator");
                return StatusCode(500, ModelState);
            }
            return Ok("ny Commentator created");
        }

        [HttpPut("{commentatorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatedCommentator(int commentatorId, 
            [FromBody] CommentatorDto updatedCommentator)
        {
            if (updatedCommentator == null)
                return BadRequest(ModelState);

            if (commentatorId != updatedCommentator.Id)
                return BadRequest(ModelState);

            if (!_commentatorRepository.CommentatorExists(commentatorId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var commentatorMap = _mapper.Map<Commentator>(updatedCommentator);

            if (!_commentatorRepository.UpdateCommentator(commentatorMap))
            {
                ModelState.AddModelError("", "Something wrong with updating commentator");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{commentatorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int commentatorId)
        {
            if (!_commentatorRepository.CommentatorExists(commentatorId))
            {
                return NotFound();
            }

            var commentatorToDelete = _commentatorRepository.GetCommentator(commentatorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_commentatorRepository.DeleteCommentator(commentatorToDelete))
            {
                ModelState.AddModelError("", "Something wrong with deleting commentator");
            }

            return NoContent();
        }
    }
}
