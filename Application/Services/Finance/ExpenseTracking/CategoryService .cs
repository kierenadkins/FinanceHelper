using FinanceHelper.Application.Data;
using FinanceHelper.Application.Services.Cache;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Application.Services.Finance.ExpenseTracking;

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
            ctx => ctx.Categories.AsNoTracking().Where(c => c.UserId == userId && c.Deleted == false),
            userId
        );
    }
    public Task<List<Category>> GetAllCategoriesWithSubCategoriesWithUserIdCached(int userId)
    {
        return GetListCachedAsync(
            ctx => ctx.Categories
                .AsNoTracking()
                .Include(c => c.SubCategories.Where(sc => sc.Deleted == false))
                .Where(c => c.UserId == userId && c.Deleted == false),
            userId
        );
    }
}