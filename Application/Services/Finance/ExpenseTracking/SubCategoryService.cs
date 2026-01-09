using Application.Data;
using Application.Domain.Finance.ExpenseTracking;
using Application.Services.Cache;

namespace Application.Services.Finance.ExpenseTracking;

public class SubCategoryService : GenericCrudService<SubCategory>, ISubCategoryService
{
    public SubCategoryService(
        IRepository<SubCategory> repo,
        ICacheManagerService cache,
        LocalDbContext context,
        IEntityCacheKey<SubCategory> cacheKeys)
        : base(repo, cache, context, cacheKeys)
    {
    }
}