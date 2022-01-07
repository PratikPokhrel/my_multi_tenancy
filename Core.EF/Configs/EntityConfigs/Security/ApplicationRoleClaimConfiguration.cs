using Core.EF.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EF.Configs.EntityConfigs
{
    public class ApplicationRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.ToTable("RoleClaim", "dbo");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.RoleId).HasColumnName("RoleID");
            builder.Property(e => e.ClaimValue).HasColumnName("ClaimValue").HasMaxLength(50);

            builder.HasOne(e=>e.ApplicationRole)
                   .WithMany(e=>e.ApplicationRoleClaims)
                   .HasForeignKey(e=>e.RoleId);
        }
    }
}
