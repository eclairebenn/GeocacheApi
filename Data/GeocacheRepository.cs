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
        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }


        //Geocaches
        public IEnumerable<Geocache> GetAllGeocaches()
        {
            _logger.LogInformation("GetAllGeocaches was run");

                return _context.Geocaches
                .OrderBy(g => g.Name)
                .Include(g => g.Items)
                .Include(g => g.Location)
                .ToList();

        }

        public Geocache GetGeocacheById(int id)
        {
            return _context.Geocaches
                .Where(g => g.ID == id)
                .Include(g => g.Items)
                .Include(g => g.Location)
                .First();
        }

        //Items
        public IEnumerable<Item> GetAllItems()
        {
            return _context.Items.ToList();
        }

        public Item GetItemById(int id)
        {
            return _context.Items
                .Where(g => g.Id == id)
                .Include(i => i.Geocache)
                .First();
        }
    }
}
