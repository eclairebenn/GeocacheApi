namespace GeocacheAPI.Data
{
    public interface IGeocacheRepository
    {
        void AddEntity(object model);
        bool SaveAll();

        IEnumerable<Geocache> GetAllGeocaches();
        Geocache GetGeocacheById(int id);

        IEnumerable<Item> GetAllItems();
        Item GetItemById(int id);
    }
}