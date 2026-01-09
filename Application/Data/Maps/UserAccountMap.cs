using Application.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

using System.Text;

namespace Application.Data.Maps
{
    public class UserAccountMap : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable("tbl_UserAccount");
            builder.HasKey(x => x.Id);
        }
    }
}
