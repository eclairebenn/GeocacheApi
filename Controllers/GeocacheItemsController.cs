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
        public IActionResult Get(int geocacheid)
        {
            var geocache = _repository.GetGeocacheById(geocacheid);
            var items = geocache.Items;
            if (items != null)
            {
                return Ok(_mapper.Map<IEnumerable<Item>, IEnumerable<ItemViewModel>>(items));
            }
            return NotFound();
        }

        /// <summary>
        /// Get Item by Id from Geocache
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult Get(int geocacheid, int id)
        {
            var geocache = _repository.GetGeocacheById(geocacheid);
            var items = geocache.Items;
            if (items != null)
            {
                var item = items.Where(i => i.Id == id).FirstOrDefault();
                if (item != null)
                {
                    return Ok(_mapper.Map<Item, ItemViewModel>(item));
                }
            }
            return NotFound();
        }


        /// <summary>
        /// Add New Item To Geocache By Id
        /// </summary>
        [HttpPost]

        public IActionResult Post(int geocacheid, [FromBody]ItemViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var geocache = _repository.GetGeocacheById(geocacheid);
                    if (geocache != null)
                    {
                        //Check length of current geocache items
                        //if <3 - create item and update geocache obj
                        var newItem = _mapper.Map<ItemViewModel, Item>(model);

                        if (newItem.Activated == DateTime.MinValue)
                        {
                            newItem.Activated = DateTime.Now;
                        }

                        _repository.AddEntity(newItem);
                        if (_repository.SaveAll())
                        {
                            return Created($"/api/items/{newItem.Id}", _mapper.Map<Item, ItemViewModel>(newItem));
                        }
                    }
                    
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new Item: {ex}");
                return BadRequest($"Failed to save a new Item: {ex}");
            }
            return BadRequest("Failed");

        }

        //Remove Existing Item from Geocache List
        //Add Existing Item to Geocache List If Active


    }
}
