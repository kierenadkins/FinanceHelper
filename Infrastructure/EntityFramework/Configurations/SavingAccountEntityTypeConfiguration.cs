using FinanceHelper.Domain.Objects.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHelper.Infrastructure.EntityFramework.Configurations;

public class SavingAccountEntityTypeConfiguration : IEntityTypeConfiguration<SavingAccount>
{
    public void Configure(EntityTypeBuilder<SavingAccount> builder)
    {
        builder.ToTable("tbl_SavingAccount");
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Transactions).WithOne()
            .HasForeignKey(x => x.SavingAccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}