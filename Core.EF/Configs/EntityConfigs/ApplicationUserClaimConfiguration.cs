using Core.EF.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.EF.Configs.EntityConfigs
{


    public class ApplicationUserClaimConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            builder.ToTable("UserClaim", "dbo");
            builder.Property(e => e.UserId).HasColumnName("UserID");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.ClaimValue).HasColumnName("ClaimValue").HasMaxLength(50);
        }
    }
}
