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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;// Execute a mapping from the source object to a new destination object with supplied
        //     mapping options.
        //nemlig, match dto with main class and do things via dto

        //inject interface and automapper
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            //return a list, otherwise you get problems
            var categories = _mapper.Map<List<CategoryDto>>
                (_categoryRepository.GetCategories());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId)) { return NotFound(); }
            var category = _mapper
                    .Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByCategoryId(int categoryId)
        {
            var pokemons = _mapper
                    .Map<List<PokemonDto>>(
                _categoryRepository.GetPokemonByCategory(categoryId));
            if (!ModelState.IsValid) { return BadRequest(); }  
            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]//The 204 (No Content) status code indicates that the server has successfully fulfilled the request and that there is no additional content to send in the response payload body. While 200 OK being a valid and the most common answer, returning a 204 No Content could make sense as there is absolutely nothing to return.
        [ProducesResponseType(400)]//The HyperText Transfer Protocol (HTTP) 400 Bad Request response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing).
        public IActionResult CreateCategory([FromBody] CategoryDto creatNyCategory)
        {
        if (creatNyCategory == null) return BadRequest();

        var category = _categoryRepository.GetCategories()
                .Where(c=>c.Name.Trim().ToUpper()== creatNyCategory.Name.Trim().ToUpper()).FirstOrDefault();
            
            if (category != null) 
            {
                ModelState.AddModelError("", "already exists..not a new category");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            //error#01:Missing type map configuration or unsupported mapping.
            //solution#01:need to GO mapprofile, and map category 2 dto AND dto 2 category
            var mapNyCategory = _mapper.Map<Category>(creatNyCategory);

            if (!_categoryRepository.CreateCategory(mapNyCategory))
            {
                ModelState.AddModelError("", "failed saving category");
                return StatusCode(500, ModelState);
            }
            return Ok("ny kategori created");
        }

    }
}
