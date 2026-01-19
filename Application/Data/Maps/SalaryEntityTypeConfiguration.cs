using Application.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

using System.Text;
using Application.Domain.Finance;

namespace Application.Data.Maps
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
