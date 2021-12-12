using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EF.Configs.EntityConfigs.Tenants
{

    public class TenantEntityConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Identifier).HasMaxLength(50);
            builder.HasData(
            new Tenant
            {
                Id = Guid.Parse("4249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"),
                Server = "localhost",
                Identifier= "sme1:5001",
                Database = "multitenant_dev_db",
                User = "postgres",
                Password = "Admin",
                Port = "5432",
                DatabaseType = 1
            },
            new Tenant
            {
                Id = Guid.Parse("5249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"),
                Server = "localhost",
                Identifier = "sme2:5001",
                Database = "multitenant1_dev_db",
                User = "postgres",
                Password = "Admin",
                Port = "5432",
                DatabaseType = 1
            });
        }
    }
}
