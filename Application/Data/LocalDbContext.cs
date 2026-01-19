using Application.Data.Maps;
using Application.Domain.Users;
using Application.Domain.Finance;
using Application.Domain.Finance.ExpenseTracking;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    public class LocalDbContext : DbContext
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
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocalDbContext).Assembly);
        }
    }
}