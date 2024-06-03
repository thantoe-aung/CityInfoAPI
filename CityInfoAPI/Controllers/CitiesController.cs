﻿using AutoMapper;
using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maximumPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository,IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

     

        [HttpGet]
        public async Task<ActionResult> GetCities([FromQuery(Name = "name")]string? name,string? searchQuery,
            int pageNumber =1,int pageSize = 10)
        {
            if(pageSize > maximumPageSize)
            {
                pageSize = maximumPageSize;
            }

            var (cityEntities,paginationData) = await _cityInfoRepository.GetCitiesAsync(name,searchQuery,pageNumber,pageSize);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationData));

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id,bool includePointOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id,includePointOfInterest);
            if(city == null)
            {
                return NotFound();
            }
            if(includePointOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city));
        }
    }
}
