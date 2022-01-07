using Core.EF.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.EF.Configs.EntityConfigs
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable(name: "Role", schema: "dbo");
            builder.Property(e => e.Id);
            builder.HasIndex(x => x.NormalizedName).IsUnique(false);
            // add composite constraint 
            builder.HasIndex(x => new { x.NormalizedName, x.TenantId }).IsUnique();
        }
    }
}
