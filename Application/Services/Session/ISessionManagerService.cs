namespace Core.Services.Session
{
    public interface ISessionManagerService
    {
        void ClearAll();
        T Get<T>(string key);
        bool IsSet(string key);
        void Remove(string key);
        void Set<T>(T obj, string key);
    }
}