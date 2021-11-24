using Microsoft.AspNetCore.Mvc;
using GeocacheAPI.Data;
using GeocacheAPI.ViewModels;
using AutoMapper;

namespace GeocacheAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]

    public class GeocachesController : Controller
    {
        private readonly IGeocacheRepository _repository;
        private readonly ILogger<GeocachesController> _logger;
        private readonly IMapper _mapper;

        public GeocachesController(IGeocacheRepository repository, ILogger<GeocachesController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Geocaches
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _repository.GetAllGeocaches();
                if(result != null)
                {
                    return Ok(_mapper.Map<IEnumerable<Geocache>, IEnumerable<GeocacheViewModel>>(result));
                }

                return BadRequest("No Result value");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get geocaches: {ex}");
                return BadRequest($"Failed to get geocaches: {ex}");

            }
        }

        /// <summary>
        /// Get Geocache by Id
        /// </summary>
        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            try
            {
                var geocache = _repository.GetGeocacheById(id);

                if (geocache != null)
                {
                    return Ok(_mapper.Map<GeocacheViewModel>(geocache));
                }
                else
                {
                    return NotFound();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get geocache by ID: {ex}");
                return BadRequest($"Failed to get geocache by ID: {ex}");

            }
        }

        /// <summary>
        /// Create New Geocache
        /// </summary>
        [HttpPost]
        public IActionResult Post([FromBody] GeocacheViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newGeocache = _mapper.Map<GeocacheViewModel, Geocache>(model);

                    _repository.AddEntity(newGeocache);
                    if (_repository.SaveAll())
                    {
                        return Created($"/api/items/{newGeocache.ID}", _mapper.Map<Geocache, GeocacheViewModel>(newGeocache));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new Geocache: {ex}");
                return BadRequest($"Failed to save a new Geocache: {ex}");
            }
            return BadRequest("Fail");



        }

        /// <summary>
        /// Update Geocache by Id
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GeocacheViewModel>> Put(int id, [FromBody] GeocacheViewModel model)
        {
            try
            {
                var oldGeocache = _repository.GetGeocacheById(id);
                if(oldGeocache == null)
                {
                    return NotFound($"Could not find Geocache with ID of {id}");
                }
                _mapper.Map(model, oldGeocache);

                if (_repository.SaveAll())
                {
                    return _mapper.Map<Geocache, GeocacheViewModel>(oldGeocache);
                }

            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to update geocache by ID: {ex}");
                return BadRequest($"Failed to update geocache by ID: {ex}");

            }
            return BadRequest();
        }
    }
}
