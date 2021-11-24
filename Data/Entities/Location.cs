namespace GeocacheAPI.Data
{
    public class Location
    {
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public int GeocacheId   { get; set; }
        public Geocache? Geocache { get; set; }
    }
}
