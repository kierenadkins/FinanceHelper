
public interface IGenericCrudService<T> where T : class
{
    Task<T> AddAsync(T entity, params object[] cacheArgs);
    Task AddRangeAsync(List<T> entities, params object[] cacheArgs);
    Task DeleteAsync(int id, params object[] cacheArgs);
    Task DeleteAsync(T entity, params object[] cacheArgs);
    Task<T?> GetByIdAsync(int id);
    Task UpdateAsync(IEnumerable<T> entities, params object[] cacheArgs);
    Task UpdateAsync(T entity, params object[] cacheArgs);
}