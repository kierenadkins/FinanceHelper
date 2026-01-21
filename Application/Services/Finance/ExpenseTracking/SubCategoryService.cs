using FinanceHelper.Application.Common;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;

namespace FinanceHelper.Application.Services.Finance.ExpenseTracking;

public class SubCategoryService : GenericCrudService<SubCategory>, ISubCategoryService
{
    public SubCategoryService(
        IRepository<SubCategory> repo,
        ICacheManagerService cache,
        IFinanceHelperDbContext context,
        IEntityCacheKey<SubCategory> cacheKeys)
        : base(repo, cache, context, cacheKeys)
    {
    }
}