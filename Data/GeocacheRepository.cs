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

        public async Task<Geocache> GetGeocacheAsync(string moniker, bool includeItems = true)
        {

            _logger.LogInformation($"Getting Geocache {moniker}");
            IQueryable<Geocache> query = _context.Geocaches
                .Include(g => g.Location);

            if (includeItems)
            {
                query = query.Include(g => g.Items);
            }

            query = query.Where(g => g.Moniker == moniker);

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
            var query = _context.Items.Where(i => i.Id == id);
            return await _context.Items.FirstOrDefaultAsync();
        }
    }
}
