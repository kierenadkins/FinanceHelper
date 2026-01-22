using FinanceHelper.Application.Common;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Objects.Finance;

namespace FinanceHelper.Application.Services.Finance;

public class SalaryService(IRepository<Salary> repository,
    ICacheManagerService cacheManager,
    IFinanceHelperDbContext ctx,
    IEntityCacheKey<Salary> cacheKeys
) : GenericCrudService<Salary>(repository, cacheManager, ctx, cacheKeys), ISalaryService
{
    public async Task<Salary?> GetAllByUserIdCacheAsync(int userId)
    {
        var cacheKey = CacheKeys.SalaryByUserId(userId);

        if (cacheManager.IsSet(cacheKey))
            return cacheManager.Get<Salary>(cacheKey);

        var salary = await repository.GetAsync(x => x.UserId == userId);

        if (salary is not null)
            cacheManager.Set(salary, cacheKey, 3600);

        return salary;
    }
}