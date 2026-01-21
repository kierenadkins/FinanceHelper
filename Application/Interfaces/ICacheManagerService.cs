namespace FinanceHelper.Application.Interfaces
{
    public interface ICacheManagerService
    {
        void ClearAll();
        T Get<T>(string key);
        List<string> GetAllKeys();
        T GetOrCreate<T>(string key, Func<T> func, int expireIn = 60);
        Task<T> GetOrCreateAsync<T>(string key, Func<T> func, int expireIn = 60);
        T GetOrCreateSliding<T>(string key, Func<T> func, int expiresIn = 60);
        Task<T> GetOrCreateSlidingAsync<T>(string key, Func<T> func, int expiresIn = 60);
        bool IsSet(string key);
        void Remove(string key);
        Task<T> Set<T>(string key, T obj, int expireIn = 60);
        T Set<T>(T obj, string key, int time);
        T SetSliding<T>(T obj, string key, int time);
    }
}