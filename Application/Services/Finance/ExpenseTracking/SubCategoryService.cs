using FinanceHelper.Application.Data;
using FinanceHelper.Application.Services.Cache;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;

namespace FinanceHelper.Application.Services.Finance.ExpenseTracking;

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