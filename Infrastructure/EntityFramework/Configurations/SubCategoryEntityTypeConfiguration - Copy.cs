using FinanceHelper.Domain.Objects.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHelper.Infrastructure.EntityFramework.Configurations;

public class SavingTransactionsEntityTypeConfiguration : IEntityTypeConfiguration<SavingTransaction>
{
    public void Configure(EntityTypeBuilder<SavingTransaction> builder)
    {
        builder.ToTable("tbl_SavingTransactions");
        builder.HasKey(x => x.Id);
        builder.HasOne<SavingAccount>()
            .WithMany(c => c.Transactions)
            .HasForeignKey(x => x.SavingAccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}