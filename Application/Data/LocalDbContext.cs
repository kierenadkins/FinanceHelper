using Application.Data.Maps;
using Application.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;

namespace Application.Data
{
    public class LocalDbContext(DbContextOptions<LocalDbContext> options) : DbContext(options)
    {
        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserAccountMap());
        }
    }
}
