using AutoMapper;
using CityInfoAPI.Entities;
using CityInfoAPI.Models;

namespace CityInfoAPI.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointOfInterestDto>();
            CreateMap<City,CityDto>();
        }
    }
}
