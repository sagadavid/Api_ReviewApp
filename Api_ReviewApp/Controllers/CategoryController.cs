using Api_ReviewApp.Dto;
using Api_ReviewApp.DTO;
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
            var categories = _mapper
                //return a list, otherwise you get problems
                .Map<List<CategoryDto>>(_categoryRepository.GetCategories());
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

    }
}
