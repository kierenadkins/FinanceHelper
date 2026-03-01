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
    IEntityCacheKey<SavingAccount> cacheKeys,
    IInterestCalculationService interestCalculationService
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
        
        if (account != null)
        {
            interestCalculationService.ApplyInterestIfDue(account);
            await ctx.SaveChangesAsync();
        }
        
        return account;
    }

    public async Task<List<SavingAccount>?> GetAllByUserIdCacheAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero", nameof(userId));

        var cacheKey = CacheKeys.SavingByUserId(userId);

        // Always clear cache first to ensure fresh data
        _cache.Remove(cacheKey);

        // Fetch fresh data from database
        var saving = await ctx.Set<SavingAccount>()
            .Where(x => x.UserId == userId && !x.Deleted)
            .Include(x => x.Transactions)
            .ToListAsync();

        // Apply interest if needed
        foreach (var account in saving)
        {
            interestCalculationService.ApplyInterestIfDue(account);
        }
        
        // Save interest changes if any
        if (saving.Any(a => a.Transactions.Any(t => t.Type == TransactionType.Interest && t.TransactionDate >= DateTime.UtcNow.AddMinutes(-1))))
        {
            await ctx.SaveChangesAsync();
        }

        // Set fresh cache
        if (saving.Count > 0)
        {
            _cache.Set(saving, cacheKey, SavingAccountCacheDurationSeconds);
        }

        return saving;
    }
}
