using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _dataStore;

        public CitiesController(CitiesDataStore dataStore)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        [HttpGet]
        public ActionResult GetCities()
        {
          return Ok(_dataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult GetCity(int id)
        {
            var result = _dataStore.Cities.FirstOrDefault(x => x.Id == id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
