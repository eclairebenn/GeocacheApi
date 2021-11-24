﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly LinkGenerator _linkGenerator;

        public GeocachesController(IGeocacheRepository repository, ILogger<GeocachesController> logger, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Get all Geocaches
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<GeocacheViewModel[]>> Get(bool includeItems = true)
        {
            try
            {
                var result = await _repository.GetAllGeocachesAsync(includeItems);
                if(result != null)
                {
                    return Ok(_mapper.Map<GeocacheViewModel[]>(result));
                }

                return NotFound("No geocaches returned");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get geocaches: {ex}");
                return BadRequest($"Failed to get geocaches: {ex}");

            }
        }

        /// <summary>
        /// Get Geocache by Moniker
        /// </summary>
        [HttpGet("{moniker}")]
        public async Task<ActionResult<GeocacheViewModel>> Get(string moniker, bool includeItems = true)
        {
            try
            {
                var geocache = await _repository.GetGeocacheAsync(moniker, includeItems);

                if (geocache != null)
                {
                    return _mapper.Map<GeocacheViewModel>(geocache);
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
        public async Task<ActionResult<GeocacheViewModel>> Post([FromBody] GeocacheViewModel model)
        {
            try
            {
                var location = _linkGenerator.GetPathByAction("Get", "Geocaches", new {moniker = model.Moniker });

                if(string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current Moniker");
                }
                if (ModelState.IsValid)
                {
                    var newGeocache = _mapper.Map<Geocache>(model);

                    _repository.AddEntity(newGeocache);
                    if (await _repository.SaveAllAsync())
                    {
                        return Created(location, _mapper.Map<Geocache, GeocacheViewModel>(newGeocache));
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

            return BadRequest();

        }

        /// <summary>
        /// Update Geocache by Id
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GeocacheViewModel>> Put(string moniker, [FromBody] GeocacheViewModel model, bool includeTalks = true)
        {
            try
            {
                var oldGeocache = await _repository.GetGeocacheAsync(moniker, includeTalks);
                if(oldGeocache == null)
                {
                    return NotFound($"Could not find Geocache with moniker of {moniker}");
                }
                _mapper.Map(model, oldGeocache);

                if (await _repository.SaveAllAsync())
                {
                    return _mapper.Map<GeocacheViewModel>(oldGeocache);
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
