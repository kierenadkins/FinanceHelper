using FinanceHelper.Application.Common;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Services.Session;
using FinanceHelper.Domain.Objects.Accounts;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
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
    public Task<List<SavingAccount>> GetAllSavingsWithTransactionsWithUserIdCached(int userId)
    {
        return GetListCachedAsync(
            ctx => ctx.SavingAccounts
                .AsNoTracking()
                .Include(c => c.Transactions.Where(sc => sc.Deleted == false))
                .Where(c => c.UserId == userId && c.Deleted == false),
            userId
        );
    }
}
