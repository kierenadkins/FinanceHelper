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
    private const int SalaryCacheDurationSeconds = 3600;

    public async Task<Salary?> GetAllByUserIdCacheAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero", nameof(userId));

        var cacheKey = CacheKeys.SalaryByUserId(userId);

        if (cacheManager.IsSet(cacheKey))
            return cacheManager.Get<Salary>(cacheKey);

        var salary = await repository.GetAsync(x => x.UserId == userId);

        if (salary is not null)
            cacheManager.Set(salary, cacheKey, SalaryCacheDurationSeconds);

        return salary;
    }
}