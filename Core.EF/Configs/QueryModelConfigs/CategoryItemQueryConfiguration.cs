using Core.Entities.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EF.Configs.QueryModelConfigs
{
    public class CategoryItemQueryConfiguration : IEntityTypeConfiguration<CategoryItemQuery>
    {
        public void Configure(EntityTypeBuilder<CategoryItemQuery> builder)
        {
            builder.HasNoKey().ToView(null);
        }
    }
}
