using Application.Data.Maps;
using Application.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using Application.Domain.Finance;
using Application.Domain.Finance.ExpenseTracking;

namespace Application.Data
{
    public class LocalDbContext(DbContextOptions<LocalDbContext> options) : DbContext(options)
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Salary> Salary { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserAccountMap());
            modelBuilder.ApplyConfiguration(new SalaryMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new SubCategoryMap());
        }
    }
}
