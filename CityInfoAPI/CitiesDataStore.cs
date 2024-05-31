
using CityInfoAPI.Models;

namespace CityInfoAPI
{
    public class CitiesDataStore
    {

        public List<CityDto> Cities { get; set; }

        // public static CitiesDataStore Current { get; } = new CitiesDataStore(); 

        public CitiesDataStore() {

            Cities = new List<CityDto>()
            {
              new CityDto()
              {
                  Id = 1,
                  Name = "Bangkok",
                  Description = "Bankgkok",
                  PointOfInterests = new List<PointOfInterestDto>()
                  {
                      new PointOfInterestDto()
                      {
                          Id= 4,
                          Name ="Dream World",
                          Description = "Kids favourite"
                      }
                  }
              },

              new CityDto()
              {
                  Id = 2,
                  Name = "Yangon",
                  PointOfInterests = new List<PointOfInterestDto>()
                  {
                      new PointOfInterestDto()
                      {
                          Id= 5,
                          Name ="Shwe Dagon Pagoda",
                          Description = "Country's Famous Pagoda"
                      }
                  }
              },

              new CityDto()
              {
                  Id=3,
                  Name = "Mandalay",
                  PointOfInterests = new List<PointOfInterestDto>()
                  {
                      new PointOfInterestDto()
                      {
                          Id= 6,
                          Name ="Old Palace",
                          Description = "Amazing architect"
                      }
                  }
              }
            };
        }
    }
}
