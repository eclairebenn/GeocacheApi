namespace GeocacheAPI.Data
{
    public interface IGeocacheRepository
    {
        void AddEntity<T>(T model) where T : class;
        Task<bool> SaveAllAsync();

        Task<Geocache[]> GetAllGeocachesAsync(bool includeTalks);
        Task<Geocache> GetGeocacheAsync(string Moniker, bool includeTalks);

        Task<Item[]> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int id);
    }
}