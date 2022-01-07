using Core.EF.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.EF.Configs.EntityConfigs
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable(name: "User", schema: "dbo");
            builder.Property(e => e.Id);
            builder.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.IsActive).HasDefaultValue(true);
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
