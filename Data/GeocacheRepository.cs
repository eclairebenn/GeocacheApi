using Microsoft.EntityFrameworkCore;
namespace GeocacheAPI.Data
{
    public class GeocacheRepository : IGeocacheRepository
    {
        private readonly GeocacheContext _context;
        private readonly ILogger<GeocacheRepository> _logger;

        public GeocacheRepository(GeocacheContext context, ILogger<GeocacheRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //General
        public void AddEntity<T>(T model) where T : class
        {
            _logger.LogInformation($"Adding {model.GetType} to the context.");
            _context.Add(model);
        }

        public async Task<bool> SaveAllAsync()
        {

            _logger.LogInformation($"Saving changes in context");
            return (await _context.SaveChangesAsync()) > 0;
        }


        //Geocaches
        public async Task<Geocache[]> GetAllGeocachesAsync(bool includeItems = true)
        {
            _logger.LogInformation("Getting all geocaches");
            IQueryable<Geocache> query = _context.Geocaches
                .Include(g => g.Location);

            if(includeItems)
            {
                query = query.Include(g => g.Items);
            }

            return await query.ToArrayAsync();

        }

        public async Task<Geocache> GetGeocacheAsync(int id, bool includeItems = true)
        {

            _logger.LogInformation($"Getting Geocache {id}");
            IQueryable<Geocache> query = _context.Geocaches
                .Include(g => g.Location);

            if (includeItems)
            {
                query = query.Include(g => g.Items);
            }

            query = query.Where(g => g.ID == id);

            return await query.FirstOrDefaultAsync();

        }

        //Items
        public async Task<Item[]> GetAllItemsAsync()
        {
            _logger.LogInformation($"Getting all items");

            var query = _context.Items;

            return await query.ToArrayAsync();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            _logger.LogInformation($"Getting Item by Id");

            IQueryable<Item> query = _context.Items;

            query = query.Where(i => i.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}
