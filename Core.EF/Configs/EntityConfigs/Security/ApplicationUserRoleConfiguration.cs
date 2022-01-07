using Core.EF.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.EF.Configs.EntityConfigs
{

    public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.HasKey(e => new { e.UserId, e.RoleId });
            builder.ToTable("UserRole", "dbo");
            builder.Property(e => e.UserId).HasColumnName("UserID");
            builder.Property(e => e.RoleId).HasColumnName("RoleID");

            builder.HasOne(e => e.ApplicationUser)
                    .WithMany(e => e.ApplicationUserRoles)
                    .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.ApplicationRole)
                   .WithMany(e => e.ApplicationUserRoles)
                   .HasForeignKey(e => e.RoleId);
        }
    }
}
