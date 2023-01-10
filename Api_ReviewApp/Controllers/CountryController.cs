﻿using Api_ReviewApp.Data;
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
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;

        //because we use repository pattern,
        //controller uses irepository instead of using datacaontext directly
        //private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        //public CountryController(DataContext dataContext, IMapper mapper)
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        //remember to add countryrepository to mappingprofile !!!
        {
            //_dataContext = dataContext;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        { var countries = _mapper.Map<List<CountryDto>>
                (_countryRepository.GetCountries()).ToList();

            if (!ModelState.IsValid) { return BadRequest(ModelState);}
            return Ok(countries); 
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryById(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId)) { return NotFound(); }
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryById(countryId));//cant map boolean

            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            
            return Ok(country);
        }

        [HttpGet("/owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwnerId(int ownerId)
        {
            var country = _mapper
                    .Map<CountryDto>(_countryRepository.GetCountryByOwnerId(ownerId));
            if (!ModelState.IsValid) { return BadRequest(); }
            return Ok(country);
        }







    }

}