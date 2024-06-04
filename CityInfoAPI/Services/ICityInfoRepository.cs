using CityInfoAPI.Entities;
using CityInfoAPI.Models;

namespace CityInfoAPI.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<(IEnumerable<City>,PaginationMetaData)> GetCitiesAsync(string? name,string? searchQuery,int pageNumber,int pageSize);

        Task<City?> GetCityAsync(int cityId,bool includePointOfInterest);

        Task<bool> CityExistAsync(int cityId);

        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId);

        Task<PointOfInterest?> GetPointOfInterestsForCityAsync(int cityId,int pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId,PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        Task<bool> CityNameMatchCityId(int cityId,string? cityName);

        Task<bool> SaveChangesAsync();
    }
}
