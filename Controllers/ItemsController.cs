using Microsoft.AspNetCore.Mvc;
using GeocacheAPI.Data;
using GeocacheAPI.ViewModels;
using AutoMapper;

namespace GeocacheAPI.Controllers
{
    [Route("api/[Controller]")]
    public class ItemsController : Controller
    {
        private readonly IGeocacheRepository _repository;
        private readonly ILogger<ItemsController> _logger;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ItemsController(IGeocacheRepository repository, ILogger<ItemsController> logger, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Get All Items
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ItemViewModel[]>> Get()
        {
            try
            {
               var result = await _repository.GetAllItemsAsync();
               if(result != null)
               {
                    return Ok(_mapper.Map<ItemViewModel[]>(result));
               }
                return NotFound("No items returned");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get Item by Id
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemViewModel>> Get(int id)
        {
            try
            {
                var item = await _repository.GetItemByIdAsync(id);
                if(item != null)
                {
                    return _mapper.Map<ItemViewModel>(item);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get item by ID: {ex}");
                return BadRequest($"Failed to get item by ID: {ex}");

            }
        }

        /// <summary>
        /// Create New Item
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ItemViewModel>> Post([FromBody]ItemViewModel model)
        {
            try
            {
                var location = _linkGenerator.GetPathByAction("Get", "Item", new { id = model.Id });
                if(string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current Id");
                }
                
                if(ModelState.IsValid)
                {
                    var newItem = _mapper.Map<Item>(model);

                    if (newItem.Activated == DateTime.MinValue)
                    {
                        newItem.Activated = DateTime.Now;
                    }

                    _repository.AddEntity(newItem);

                    if (await _repository.SaveAllAsync())
                    {
                        return Created(location, _mapper.Map<Item, ItemViewModel>(newItem));
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
            return BadRequest("Fail");            
        }
    }
}
