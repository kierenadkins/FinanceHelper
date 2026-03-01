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
    private const int SavingAccountCacheDurationSeconds = 3600;

    public async Task<SavingAccount?> GetByIdWithTransactionsAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID must be greater than zero", nameof(id));

        var account = await ctx.Set<SavingAccount>()
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(x => x.Id == id);
        return account;
    }

    public async Task<List<SavingAccount>?> GetAllByUserIdCacheAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero", nameof(userId));

        var cacheKey = CacheKeys.SavingByUserId(userId);

        if (cacheManager.IsSet(cacheKey))
            return cacheManager.Get<List<SavingAccount>>(cacheKey);

        var saving = await ctx.Set<SavingAccount>()
            .Where(x => x.UserId == userId)
            .Include(x => x.Transactions)
            .ToListAsync();

        if (saving.Count > 0)
            cacheManager.Set(saving, cacheKey, SavingAccountCacheDurationSeconds);

        return saving;
    }
}
