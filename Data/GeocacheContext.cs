using Microsoft.EntityFrameworkCore;

namespace GeocacheAPI.Data
{
    public class GeocacheContext : DbContext
    {
        public GeocacheContext(DbContextOptions<GeocacheContext> options)
            : base(options) { }

        public DbSet<Geocache> Geocaches { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
   
    }
}
