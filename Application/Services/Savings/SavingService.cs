using FinanceHelper.Application.Common;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Services.Session;
using FinanceHelper.Domain.Objects.Accounts;

namespace FinanceHelper.Application.Services.Savings;

public class SavingAccountService(
    IRepository<SavingAccount> repository,
    ISessionManagerService sessionManager,
    ICacheManagerService cacheManager,
    IFinanceHelperDbContext ctx,
    IEntityCacheKey<SavingAccount> cacheKeys
) : GenericCrudService<SavingAccount>(repository, cacheManager, ctx, cacheKeys), ISavingService
{
    public async Task<List<SavingAccount>?> GetAllByUserIdCacheAsync(int userId)
    {
        var cacheKey = CacheKeys.SavingByUserId(userId);

        if (cacheManager.IsSet(cacheKey))
            return cacheManager.Get<List<SavingAccount>>(cacheKey);

        var saving = await repository.GetAllAsync(x => x.UserId == userId);

        if (saving.Count > 0)
            cacheManager.Set(saving, cacheKey, 3600);

        return saving;
    }
}
