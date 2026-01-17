using Application.Domain.Accounts;

namespace Application.Services.Finance.ExpenseTracking;

public interface ISavingService
{
    Task<SavingAccount?> GetByIdAsync(int id);
    Task<SavingAccount> AddAsync(SavingAccount entity, params object[] cacheArgs);
    Task AddRangeAsync(List<SavingAccount> entities, params object[] cacheArgs);
    Task UpdateAsync(SavingAccount entity, params object[] cacheArgs);
    Task UpdateAsync(IEnumerable<SavingAccount> entities, params object[] cacheArgs);
    Task DeleteAsync(int id, params object[] cacheArgs);
    Task DeleteAsync(SavingAccount entity, params object[] cacheArgs);
}