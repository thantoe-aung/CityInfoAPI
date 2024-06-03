using AutoMapper;
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
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointOfInterestController(ILogger<PointOfInterestController> logger,IMailService mailService,
            ICityInfoRepository repository,IMapper mapper)            
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));   
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        [HttpGet]
        public async Task<ActionResult> GetPointOfInterest(int cityId)
        {
        
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                return NotFound();
            }
            var result = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(result));
        }

        [HttpGet("{pointofinterestid}",Name = "GetPointOfInterest")]
        public async Task<ActionResult> GetPointOfInterest(int cityId,int pointofinterestid)
        {
            try
            {
                if (!await _cityInfoRepository.CityExistAsync(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} does not exist");
                    return NotFound();
                }
           
                var pointOfInterest = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointofinterestid);
                if (pointOfInterest == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));

            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception occur when getting point of interest with cityId {cityId}",ex);
                return StatusCode(500, "A problem happened when handling your request");
            }
          
        }

        [HttpPost]
        public async Task<ActionResult> CreatePointOfInterest(int cityId,[FromBody]PointOfInterestForCreationDto pointOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} does not exist");
                return NotFound();
            }

            var finalpointOfInterest= _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalpointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var result = _mapper.Map<PointOfInterestDto>(finalpointOfInterest);
           
            return CreatedAtRoute("GetPointOfInterest",new { cityId = cityId,Description = result.Description}, result);
        }


        [HttpPut("{pointOfInterestId}")]
        public  async Task<ActionResult> UpdatePointOfInterest(int cityId,int pointOfInterestId,PointOfInterestForUpdateDto updateDto)
        {
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} does not exist");
                return NotFound();
            }

           var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestId);
            if(pointOfInterestEntity == null) { 
                return NotFound();
            }

            _mapper.Map(updateDto, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId,int pointOfInterestId,JsonPatchDocument<PointOfInterestForUpdateDto> document)
        {
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} does not exist");
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId,pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            //var pathdocument = new PointOfInterestForUpdateDto
            //{
            //    Name = localResult.Name,
            //    Description = localResult.Description,
            //};

            document.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent(); 
        }



        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId,int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} does not exist");
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send("Deleted", $"Point of interest with Id {pointOfInterestId} deleted.");
            return NoContent();

        }
    }
}
