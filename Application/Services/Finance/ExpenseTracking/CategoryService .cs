using Application.Data;
using Application.Domain.Finance.ExpenseTracking;
using Application.Services.Cache;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Finance.ExpenseTracking;

public class CategoryService(
    IRepository<Category> repo,
    ICacheManagerService cache,
    LocalDbContext context,
    IEntityCacheKey<Category> cacheKeys)
    : GenericCrudService<Category>(repo, cache, context, cacheKeys), ICategoryService
{
    public Task<List<Category>> GetAllCategoriesWithUserIdCached(int userId)
    {
        return GetListCachedAsync(
            ctx => ctx.Category.AsNoTracking().Where(c => c.UserId == userId && c.Deleted == false),
            userId
        );
    }
    public Task<List<Category>> GetAllCategoriesWithSubCategoriesWithUserIdCached(int userId)
    {
        return GetListCachedAsync(
            ctx => ctx.Category
                .AsNoTracking()
                .Include(c => c.SubCategories.Where(sc => sc.Deleted == false))
                .Where(c => c.UserId == userId && c.Deleted == false),
            userId
        );
    }
}