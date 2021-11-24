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


        /// <summary>
        /// Add New Item To Geocache By Id
        /// </summary>
        //[HttpPost]

        //public IActionResult Post(int geocacheid, [FromBody]ItemViewModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var geocache = _repository.GetGeocacheById(geocacheid);
        //            if (geocache != null)
        //            {
        //                //Check length of current geocache items
        //                //if <3 - create item and update geocache obj
        //                var newItem = _mapper.Map<ItemViewModel, Item>(model);

        //                if (newItem.Activated == DateTime.MinValue)
        //                {
        //                    newItem.Activated = DateTime.Now;
        //                }

        //                _repository.AddEntity(newItem);
        //                if (_repository.SaveAll())
        //                {
        //                    return Created($"/api/items/{newItem.Id}", _mapper.Map<Item, ItemViewModel>(newItem));
        //                }
        //            }
                    
        //        }
        //        else
        //        {
        //            return BadRequest(ModelState);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Failed to save a new Item: {ex}");
        //        return BadRequest($"Failed to save a new Item: {ex}");
        //    }
        //    return BadRequest("Failed");

        //}

        //Remove Existing Item from Geocache List
        //Add Existing Item to Geocache List If Active


    }
}
