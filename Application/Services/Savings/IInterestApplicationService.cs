using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Objects.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Application.Services.Savings;

public interface IInterestApplicationService
{
    /// <summary>
    /// Applies interest to all saving accounts that have interest due
    /// Should be called by a background job/scheduler periodically
    /// </summary>
    Task ApplyDueInterestAsync();
}

public class InterestApplicationService(
    IFinanceHelperDbContext context,
    IInterestCalculationService calculationService
) : IInterestApplicationService
{
    public async Task ApplyDueInterestAsync()
    {
        var accounts = await context.Set<SavingAccount>()
            .Include(x => x.Transactions)
            .Where(x => x.InterestType != InterestType.None)
            .ToListAsync();

        foreach (var account in accounts)
        {
            calculationService.ApplyInterestIfDue(account);
        }

        if (accounts.Any(a => a.Transactions.Any(t => t.Type == TransactionType.Interest && t.TransactionDate >= DateTime.UtcNow.AddMinutes(-5))))
        {
            await context.SaveChangesAsync();
        }
    }
}
