using Application.Data;
using Application.Domain.Accounts;
using Application.Domain.Finance.ExpenseTracking;
using Application.Services.Cache;

namespace Application.Services.Finance.ExpenseTracking;

public class SavingService(
    IRepository<SavingAccount> repo,
    ICacheManagerService cache,
    LocalDbContext context,
    IEntityCacheKey<SavingAccount> cacheKeys)
    : GenericCrudService<SavingAccount>(repo, cache, context, cacheKeys), ISavingService;