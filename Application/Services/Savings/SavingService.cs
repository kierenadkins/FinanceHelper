using FinanceHelper.Application.Common;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Services.Session;
using FinanceHelper.Domain.Objects.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Application.Services.Savings;

public class SavingAccountService(
    IRepository<SavingAccount> repository,
    ISessionManagerService sessionManager,
    ICacheManagerService cacheManager,
    IFinanceHelperDbContext ctx,
    IEntityCacheKey<SavingAccount> cacheKeys
) : GenericCrudService<SavingAccount>(repository, cacheManager, ctx, cacheKeys), ISavingService
{
    public async Task<SavingAccount?> GetByIdWithTransactionsAsync(int id)
    {
        var account = await ctx.Set<SavingAccount>()
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(x => x.Id == id);
        return account;
    }

    public async Task<List<SavingAccount>?> GetAllByUserIdCacheAsync(int userId)
    {
        var cacheKey = CacheKeys.SavingByUserId(userId);

        if (cacheManager.IsSet(cacheKey))
            return cacheManager.Get<List<SavingAccount>>(cacheKey);

        var saving = await ctx.Set<SavingAccount>()
            .Where(x => x.UserId == userId)
            .Include(x => x.Transactions)
            .ToListAsync();

        if (saving.Count > 0)
            cacheManager.Set(saving, cacheKey, 3600);

        return saving;
    }
}
