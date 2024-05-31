using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointofinterest")]
    public class PointOfInterestController : ControllerBase
    {

        private readonly ILogger<PointOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _dataStore;

        public PointOfInterestController(ILogger<PointOfInterestController> logger,IMailService mailService,CitiesDataStore dataStore)            
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));   
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        [HttpGet]
        public ActionResult GetPointOfInterest(int cityId)
        {
            var result = _dataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result.PointOfInterests);
        }

        [HttpGet("{pointofinterestid}",Name = "GetPointOfInterest")]
        public ActionResult GetPointOfInterest(int cityId,int pointofinterestid)
        {
            try
            {
                var result = _dataStore.Cities.FirstOrDefault(x => x.Id == cityId);
                if (result == null)
                {
                    _logger.LogInformation($"City with id {cityId} does not exist");
                    return NotFound();
                }

                var pointOfInterest = result.PointOfInterests.FirstOrDefault(x => x.Id == pointofinterestid);
                if (pointOfInterest == null)
                {
                    return NotFound();
                }
                return Ok(pointOfInterest);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception occur when getting point of interest with cityId {cityId}",ex);
                return StatusCode(500, "A problem happened when handling your request");
            }
          
        }

        [HttpPost]
        public ActionResult CreatePointOfInterest(int cityId,[FromBody]PointOfInterestForCreationDto pointOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _dataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (result == null)
            {
                return NotFound();
            }

            var maxId= _dataStore.Cities.SelectMany(x=> x.PointOfInterests).Max(y => y.Id);

            var insertModel = new PointOfInterestDto
            {
                Id = ++maxId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            result.PointOfInterests.Add(insertModel);

            return CreatedAtRoute("GetPointOfInterest",new { cityId = cityId,Description = insertModel.Description},insertModel);
        }


        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId,int pointOfInterestId,PointOfInterestForUpdateDto updateDto)
        {
            var result = _dataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (result == null)
            {
                return NotFound();
            }

            var localResult = result.PointOfInterests.Where(x=> x.Id == pointOfInterestId).FirstOrDefault();
            if(localResult == null) { 
                return NotFound();
            }

            localResult.Name = updateDto.Name;
            localResult.Description = updateDto.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId,int pointOfInterestId,JsonPatchDocument<PointOfInterestForUpdateDto> document)
        {
            var result = _dataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (result == null)
            {
                return NotFound();
            }

            var localResult = result.PointOfInterests.Where(x => x.Id == pointOfInterestId).FirstOrDefault();
            if (localResult == null)
            {
                return NotFound();
            }

            var pathdocument = new PointOfInterestForUpdateDto
            {
                Name = localResult.Name,
                Description = localResult.Description,
            };

            document.ApplyTo(pathdocument,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pathdocument))
            {
                return BadRequest(ModelState);
            }

            localResult.Name = pathdocument.Name;
            localResult.Description = pathdocument.Description;

            return NoContent(); 
        }



        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId,int pointOfInterestId)
        {
            var result = _dataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (result == null)
            {
                return NotFound();
            }

            var localResult = result.PointOfInterests.Where(x => x.Id == pointOfInterestId).FirstOrDefault();
            if (localResult == null)
            {
                return NotFound();
            }

            result.PointOfInterests.Remove(localResult);
            _mailService.Send("Deleted", $"Point of interest with Id {pointOfInterestId} deleted.");
            return NoContent();

        }
    }
}
