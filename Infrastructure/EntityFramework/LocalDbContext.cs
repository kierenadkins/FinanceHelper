using FinanceHelper.Application.Interfaces;
using FinanceHelper.Domain.Objects.Finance;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using FinanceHelper.Domain.Objects.Users;
using Microsoft.EntityFrameworkCore;

namespace FinanceHelper.Infrastructure.Data
{
    public class LocalDbContext : DbContext, IFinanceHelperDbContext
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocalDbContext).Assembly);
        }
    }
}