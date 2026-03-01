using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Objects.Accounts;
using FinanceHelper.Domain.Objects.Finance;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using FinanceHelper.Domain.Objects.Users;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Infrastructure.EntityFramework
{
    public class LocalDbContext(DbContextOptions<LocalDbContext> options) : DbContext(options), IFinanceHelperDbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<SavingAccount> SavingAccount { get; set; }
        public DbSet<SavingTransaction> SavingTransaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocalDbContext).Assembly);
        }
    }
}