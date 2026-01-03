using Application.Data;
using Application.Domain.Base;
using Application.Services;
using Application.Services.Cache;
using Microsoft.EntityFrameworkCore;

public abstract class GenericCrudService<T> where T : class
{
    protected readonly IRepository<T> _repo;
    protected readonly ICacheManagerService _cache;
    protected readonly LocalDbContext _context;
    protected readonly IEntityCacheKey<T> _cacheKeys;

    protected GenericCrudService(
        IRepository<T> repo,
        ICacheManagerService cache,
        LocalDbContext context,
        IEntityCacheKey<T> cacheKeys)
    {
        _repo = repo;
        _cache = cache;
        _context = context;
        _cacheKeys = cacheKeys;
    }

    protected async Task<List<T>> GetListCachedAsync(
        Func<LocalDbContext, IQueryable<T>> query,
        params object[] cacheArgs)
    {
        var key = _cacheKeys.ListKey(cacheArgs);

        if (_cache.IsSet(key))
            return _cache.Get<List<T>>(key);

        var results = await query(_context).ToListAsync();

        if (results.Any())
            _cache.Set(results, key, 3600);

        return results;
    }

    public virtual Task<T?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

    public virtual async Task<T> AddAsync(T entity, params object[] cacheArgs)
    {
        LastUpdated(entity);
        await _repo.AddAsync(entity);
        Invalidate(cacheArgs);

        return entity;
    }

    public async Task AddRangeAsync(List<T> entities, params object[] cacheArgs)
    {
        if (entities.Count == 0)
            return;

        foreach (var entity in entities)
        {
            LastUpdated(entity);
        }

        await _repo.AddRangeAsync(entities);
        Invalidate(cacheArgs);
    }

    public virtual async Task UpdateAsync(T entity, params object[] cacheArgs)
    {
        LastUpdated(entity);
        await _repo.UpdateAsync(entity);
        Invalidate(cacheArgs);
    }

    public virtual async Task UpdateAsync(IEnumerable<T> entities, params object[] cacheArgs)
    {
        if (entities == null || !entities.Any())
            return;

        foreach (var entity in entities)
        {
            if (entity is BaseEntity baseEntity)
            {
                LastUpdated(entity);
            }
        }

        await _repo.AddRangeAsync(entities);
        Invalidate(cacheArgs);
    }

    public virtual async Task DeleteAsync(int id, params object[] cacheArgs)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"Entity with Id {id} not found.");

        await DeleteAsync(entity, cacheArgs); // marks deleted if possible
    }

    public virtual async Task DeleteAsync(T entity, params object[] cacheArgs)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.MarkDeleted();
            await _repo.UpdateAsync(entity);
        }
        else
        {
            await _repo.DeleteAsync(entity);
        }

        Invalidate(cacheArgs);
    }

    protected void LastUpdated(T entity)
    {
        var now = DateTime.Now;

        if (entity is BaseEntity baseEntity)
        {
            baseEntity.LastUpdated = now;
        }
    }
    protected void Invalidate(params object[] args)
    {
        _cache.Remove(_cacheKeys.ListKey(args));
    }
}