using FinanceHelper.Application.Common;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Objects.Base;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Application.Services;

public abstract class GenericCrudService<T> : IGenericCrudService<T> where T : class
{
    protected readonly IRepository<T> _repo;
    protected readonly ICacheManagerService _cache;
    protected readonly IFinanceHelperDbContext _context;
    protected readonly IEntityCacheKey<T> _cacheKeys;

    protected GenericCrudService(
        IRepository<T> repo,
        ICacheManagerService cache,
        IFinanceHelperDbContext context,
        IEntityCacheKey<T> cacheKeys)
    {
        _repo = repo;
        _cache = cache;
        _context = context;
        _cacheKeys = cacheKeys;
    }

    protected async Task<List<T>> GetListCachedAsync(
        Func<IFinanceHelperDbContext, IQueryable<T>> query,
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

    public virtual Task<T?> GetByIdAsync(int id) => _repo.GetAsync(id);

    public virtual async Task<T> AddAsync(T entity, params object[] cacheArgs)
    {
        LastUpdated(entity);
        await _repo.InsertAsync(entity);

        if (cacheArgs.Length > 0)
        {
            Invalidate(cacheArgs);
        }

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

        await _repo.InsertManyAsync(entities);
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

        await _repo.InsertManyAsync(entities);
        Invalidate(cacheArgs);
    }

    public virtual async Task DeleteAsync(int id, params object[] cacheArgs)
    {
        var entity = await _repo.GetAsync(id);
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
            await _repo.DeletePermanentAsync(entity);
        }

        Invalidate(cacheArgs);
    }

    protected void LastUpdated(T entity)
    {
        var now = DateTime.Now;

        if (entity is BaseEntity baseEntity)
        {
            baseEntity.DateCreated = now;
            baseEntity.LastUpdated = now;
        }
    }
    protected void Invalidate(params object[] args)
    {
        _cache.Remove(_cacheKeys.ListKey(args));
    }
}