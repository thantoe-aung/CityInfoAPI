using CityInfoAPI.DbContexts;
using CityInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfoAPI.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext context;

        public CityInfoRepository(CityInfoContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await context.Cities.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<bool> CityExistAsync(int cityId)
        {
            return await context.Cities.AnyAsync(x => x.Id == cityId);
        }

        public Task<City?> GetCityAsync(int cityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return context.Cities.Include( x=> x.PointOfInterests).Where(y => y.Id == cityId).FirstOrDefaultAsync();
            }

            return context.Cities.Where(x=> x.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId)
        {
            return await context.PointOfInterest.Where(x=> x.CityId == cityId).ToListAsync();
        }

        public Task<PointOfInterest?> GetPointOfInterestsForCityAsync(int cityId, int pointOfInterestId)
        {
            return context.PointOfInterest.Where(x=> x.CityId == cityId && x.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId,false);
            if(city != null)
            {
                city.PointOfInterests.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            context.PointOfInterest.Remove(pointOfInterest);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery)
        {
            if(String.IsNullOrEmpty(name) && String.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetCitiesAsync();
            }

            var collection = context.Cities as IQueryable<City>;

            if (!String.IsNullOrEmpty(name))
            {
                name = name.Trim();
                collection = collection.Where(x => x.Name == name);
            }

            if(!String.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x=> x.Name.Contains(searchQuery) ||
                            (x.Description != null && x.Description.Contains(searchQuery)));
            }

            return await collection.OrderBy(y => y.Name).ToListAsync();
       

        }
    }
}
