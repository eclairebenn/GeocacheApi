using Microsoft.EntityFrameworkCore;

namespace GeocacheAPI.Data
{
    public class GeocacheSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var _context = new GeocacheContext(
                serviceProvider.GetRequiredService<DbContextOptions<GeocacheContext>>()))
            {
                _context.Database.EnsureCreated();

                if (_context.Geocaches.Any())
                {
                    return;
                }

                _context.Geocaches.AddRange(
                    new Geocache()
                    {
                        Name =  "Gas Works",
                        Location =  new Location() 
                        {
                            Latitude = -234, 
                            Longitude = -78
                        },
                        Items = new List<Item>()
                        {
                            new Item()
                            {
                                Name = "Teddy Bear",
                                Activated = DateTime.Parse("2012-2-12")
                            },
                            new Item()
                            {
                                Name = "Head Band",
                                Activated = DateTime.Parse("2016-4-1")
                            }
                        }
                    },
                    new Geocache()
                    {
                        Name = "Space Needle",
                        Location = new Location()
                        {
                            Latitude = -234,
                            Longitude = -78
                        },
                        Items = new List<Item>()
                        {
                            new Item()
                            {
                                Name = "Beanie Bag",
                                Activated = DateTime.Parse("2015-11-12")
                            },
                            new Item()
                            {
                                Name = "YoYo",
                                Activated = DateTime.Parse("2021-10-1")
                            }
                        }
                    },
                    new Geocache()
                    {
                        Name = "Mt. Baker",
                        Location = new Location()
                        {
                            Latitude = -234,
                            Longitude = -78
                        }
                    },
                    new Geocache()
                    {
                        Name = "Mercer Island",
                        Location = new Location()
                        {
                            Latitude = -234,
                            Longitude = -78
                        }
                    }

                    );

                _context.SaveChanges();
            } 
        }
    }
}
