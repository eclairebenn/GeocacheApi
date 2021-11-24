namespace GeocacheAPI.Data
{
    public class Geocache
    {
        public int ID { get; set; }
        public string? Name { get; set; }

        public string? Moniker { get; set; }
        public Location? Location { get; set; }
        public ICollection<Item>? Items { get; set; }

    }
}
