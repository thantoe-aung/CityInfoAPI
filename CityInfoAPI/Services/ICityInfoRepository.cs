using CityInfoAPI.Entities;

namespace CityInfoAPI.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<IEnumerable<City>> GetCitiesAsync(string? name,string? searchQuery);

        Task<City?> GetCityAsync(int cityId,bool includePointOfInterest);

        Task<bool> CityExistAsync(int cityId);

        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId);

        Task<PointOfInterest?> GetPointOfInterestsForCityAsync(int cityId,int pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId,PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        Task<bool> SaveChangesAsync();
    }
}
