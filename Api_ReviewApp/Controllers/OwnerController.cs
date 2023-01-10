using Api_ReviewApp.Dto;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api_ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {

            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpGet("ownerId")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound();
            var owner = _mapper.Map<OwnerDto>
                (_ownerRepository.GetOwner(ownerId));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(owner);

        }

        [HttpGet("{ownerId}/pokemon")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwnerId(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId)) return NotFound();

            //dto is a real data filter ! observe here.
            //var pokemon = _mapper.Map<List<PokemonDto>>
            //        (_ownerRepository.GetPokemonByOwnerId(ownerId));
            var owner = _mapper.Map<List<PokemonDto>>(
                _ownerRepository.GetPokemonByOwnerId(ownerId));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(owner);
        }

        //[HttpGet]
        //[ProducesResponseType(200, Type = typeof(Owner))]
        //public IActionResult GetOwnerByPokemonId(int pokeId)
        //{
        //    if (!_ownerRepository.OwnerExists(pokeId)) return NotFound();
        //    var owner = _mapper.Map<OwnerDto>
        //            (_ownerRepository.GetOwnerByPokemonId(pokeId));
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    return Ok(owner);
        //}
    }
}
