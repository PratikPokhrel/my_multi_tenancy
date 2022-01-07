using Core.EF.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.EF.Configs.EntityConfigs
{
    public class ApplicationUserTokenConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
        {
            builder.HasKey(e => e.UserId);
            builder.ToTable("UserToken", "dbo");
            builder.Property(e => e.UserId).HasColumnName("UserID");
        }
    }
}
