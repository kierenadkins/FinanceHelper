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
   
}
