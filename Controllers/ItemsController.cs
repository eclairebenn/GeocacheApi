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

        public ItemsController(IGeocacheRepository repository, ILogger<ItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Items
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
               var result = _repository.GetAllItems();
               return Ok(_mapper.Map<IEnumerable<ItemViewModel>>(result));
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
        public IActionResult Get(int id)
        {
            try
            {
                var item = _repository.GetItemById(id);
                if(item != null)
                {
                    return Ok(_mapper.Map<Item, ItemViewModel>(item));
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
        public IActionResult Post([FromBody]ItemViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
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
