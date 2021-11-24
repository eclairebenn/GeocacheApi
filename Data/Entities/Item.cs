namespace GeocacheAPI.Data
{
    public class Item
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Geocache? Geocache { get; set; }
        public DateTime Activated { get; set; }
        public DateTime Deactivated { get; set; }

    }
}
