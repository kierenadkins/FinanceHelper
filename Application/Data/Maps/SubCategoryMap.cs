using Application.Domain.Finance.ExpenseTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Maps;

public class SubCategoryMap : IEntityTypeConfiguration<SubCategory>
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