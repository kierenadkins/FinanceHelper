
public interface IGenericCrudService<T> where T : class
{
    public Task<T> AddAsync(T entity, params object[] cacheArgs);
    public Task AddRangeAsync(List<T> entities, params object[] cacheArgs);
    public Task DeleteAsync(int id, params object[] cacheArgs);
    public Task DeleteAsync(T entity, params object[] cacheArgs);
    public Task<T?> GetByIdAsync(int id);
    public Task UpdateAsync(IEnumerable<T> entities, params object[] cacheArgs);
    public Task UpdateAsync(T entity, params object[] cacheArgs);
}