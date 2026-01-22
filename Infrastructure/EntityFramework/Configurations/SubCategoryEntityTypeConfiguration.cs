using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHelper.Infrastructure.EntityFramework.Configurations;

public class SubCategoryEntityTypeConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.ToTable("tbl_SubCategory");
        builder.HasKey(x => x.Id);
        builder.HasOne<Category>()
            .WithMany(c => c.SubCategories)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}