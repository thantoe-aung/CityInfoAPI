using AutoMapper;
using CityInfoAPI.Entities;
using CityInfoAPI.Models;

namespace CityInfoAPI.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
            CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
            CreateMap<PointOfInterest,PointOfInterestForUpdateDto>();
        }
    }
}
