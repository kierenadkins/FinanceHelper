using FinanceHelper.Domain.Objects.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHelper.Infrastructure.EntityFramework.Configurations
{
    public class SalaryEntityTypeConfiguration : IEntityTypeConfiguration<Salary>
    {
        public void Configure(EntityTypeBuilder<Salary> builder)
        {
            builder.ToTable("tbl_Salary");
            builder.HasKey(x => x.Id);
        }
    }
}
