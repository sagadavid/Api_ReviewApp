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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        //solution#03 add country into play (for post method)
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, 
            ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner
            (//solution#03 get country id from query
            [FromQuery] int countryId,
            [FromBody] OwnerDto creatNyOwner
            //[FromQuery] OwnerDto creatNyOwner
            )
        {
            if (creatNyOwner == null) return BadRequest();

            var owner = _ownerRepository.GetOwners()
                    .Where(c => c.LastName.Trim().ToUpper() 
                    == creatNyOwner.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "already exists..not a new owner");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var mapNyOwner = _mapper.Map<Owner>(creatNyOwner);
            
            //solution#03 map countryId to owner's country
            //by enabling getcountrybyid method of countryrepository
            //mapper can catch country details, 'cause this controller
            //has constructer injection of icountryrepository
            mapNyOwner.Country = _countryRepository.GetCountryById(countryId);

            if (!_ownerRepository.CreateOwner(mapNyOwner))
            {
                ModelState.AddModelError("", "failed saving ny owner");
                return StatusCode(500, ModelState);
            }
            return Ok("ny owner created");
        }
    }
}
