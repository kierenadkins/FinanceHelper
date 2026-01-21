using FinanceHelper.Domain.Objects.Finance;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using FinanceHelper.Domain.Objects.Users;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Application.Interfaces;

public interface IFinanceHelperDbContext
{
    DbSet<UserAccount> UserAccounts { get; set; }
    DbSet<Salary> Salaries { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<SubCategory> SubCategories { get; set; }

    DbSet<T> Set<T>() where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}