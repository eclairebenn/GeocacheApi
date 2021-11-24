using AutoMapper;
using GeocacheAPI.Data;
using GeocacheAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GeocacheAPI.Controllers
{
    [Route("/api/geocaches/{geocacheid}/items")]
    public class GeocacheItemsController : Controller
    {
        private readonly IGeocacheRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public GeocacheItemsController(IGeocacheRepository repository, ILogger<GeocacheItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Items of Geocache
        /// </summary>
        
        //GET: api/geocaches/{geocacheid}/items
        [HttpGet]
        public async Task<ActionResult<ItemViewModel[]>> Get(int geocacheid, bool includeItems = true)
        {
            var geocache = await _repository.GetGeocacheAsync(geocacheid, includeItems);
            var items = geocache.Items;
            if (items != null)
            {
                return _mapper.Map<ItemViewModel[]>(items);
            }
            return NotFound();
        }

        /// <summary>
        /// Get Item by Id from Geocache
        /// </summary>
        
        //GET: api/geocaches/{geocacheid}/items/{itemid}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemViewModel>> Get(int geocacheid, int id)
        {
            try
            {
                var geocache = await _repository.GetGeocacheAsync(geocacheid, true);
                var items = geocache.Items;
                if (items != null)
                {
                    var item = await _repository.GetItemByIdAsync(id);
                    if (item != null)
                    {
                        return _mapper.Map<ItemViewModel>(item);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get item by ID from geocach: {ex}");
                return BadRequest($"Failed to get item by ID from geocache: {ex}");
            }
        }

        ///<summary>
        ///Remove Existing Item from Geocache List
        /// </summary>
        
        //PUT: api/geocaches/{geocacheid}/items/remove{itemid}
        [HttpPut("remove/{id:int}")]
        public async Task<ActionResult<GeocacheViewModel>> PutRemove(int geocacheid, int id)
        {
            try
            {
                var oldGeocache = await _repository.GetGeocacheAsync(geocacheid, true);

                var items = oldGeocache.Items;
                if (oldGeocache.Items != null)
                {
                    if (items != null)
                    {
                        var item = await _repository.GetItemByIdAsync(id);
                        if (item != null)
                        {
                            _mapper.Map<ItemViewModel>(item);
                            _mapper.Map<GeocacheViewModel>(oldGeocache);

                            oldGeocache.Items.Remove(item);
                            item.Geocache = null;
                            if (await _repository.SaveAllAsync())
                            {
                                return _mapper.Map<GeocacheViewModel>(oldGeocache);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update geocache by ID: {ex}");
                return BadRequest($"Failed to update geocache by ID: {ex}");

            }
            return BadRequest();
        }


        /// <summary>
        /// Move Existing Item to Geocache List If Item is Active && If Geocache Items are not full
        /// </summary>

        //PUT: api/geocaches/{geocacheid}/items/add/{itemid}
        [HttpPut("add/{id:int}")]
        public async Task<ActionResult<GeocacheViewModel>> PutAdd(int geocacheid, int id, bool includeTalks = true)
        {
            try
            {         
                var oldGeocache = await _repository.GetGeocacheAsync(geocacheid, includeTalks);

                var items = oldGeocache.Items;
                if (oldGeocache.Items != null)
                {
                    if (items != null)
                    {
                        var item = await _repository.GetItemByIdAsync(id);
                        if (item != null)
                        {
                            if (item.Deactivated != DateTime.MinValue)
                            {
                                return BadRequest("Item is deactivated and cannot be added to this Geocache");
                            }
                            if (oldGeocache.Items.Count == 3)
                            {
                                return BadRequest("Geocache contains maxiumum number of items");
                            }
                            _mapper.Map<ItemViewModel>(item);
                            _mapper.Map<GeocacheViewModel>(oldGeocache);

                            oldGeocache.Items.Add(item);
                            item.Geocache = oldGeocache;
                                if (await _repository.SaveAllAsync())
                                {
                                    return _mapper.Map<GeocacheViewModel>(oldGeocache);
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update geocache by ID: {ex}");
                return BadRequest($"Failed to update geocache by ID: {ex}");

            }
            return BadRequest();
        }

    }
}
