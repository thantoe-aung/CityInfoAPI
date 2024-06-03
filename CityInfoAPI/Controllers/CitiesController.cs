using AutoMapper;
using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository,IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

     

        [HttpGet]
        public async Task<ActionResult> GetCities([FromQuery(Name = "name")]string? name,string? searchQuery)
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync(name,searchQuery);
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
