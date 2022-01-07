using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EF.Configs.EntityConfigs.Security
{
    public static class SecurityModelBuilder
    {
        public static  void ConfigureSecurityEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserLoginConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationRoleClaimConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserTokenConfiguration());
        }
    }
}
