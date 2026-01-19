using Application.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Maps;

//public class SavingAccountMap : IEntityTypeConfiguration<SavingAccount>
//{
//    public void Configure(EntityTypeBuilder<SavingAccount> builder)
//    {
//        builder.ToTable("tbl_SavingAccount");
//        builder.HasKey(x => x.Id);

//        builder.HasMany(x => x.Transactions).WithOne()
//            .HasForeignKey(x => x.SavingAccountId)
//            .OnDelete(DeleteBehavior.Cascade);
//    }
//}