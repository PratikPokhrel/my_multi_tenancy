using Core.EF.Configs.EntityConfigs;
using Core.EF.Configs.EntityConfigs.Tenants;
using Core.EF.Data.Extensions;
using Core.EF.IdentityModels;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.EF.Data.Context.Default
{
    public class DefaultContext:IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>,IDbContext
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="dataSeeder">Initial data seeder</param>
        public DefaultContext(DbContextOptions<DefaultContext> options)
            : base(options)
        {
            // TODO: Comment below this if you are running migrations commands
            // TODO: uncomment below line of you are running the application for the first time
            //this.Database.EnsureCreated();
        }


        public DbContextType GetContextType => DbContextType.Account;

        /// <summary>
        /// Get or sets the devices data model
        /// </summary>

        public virtual DbSet<Tenant> Tenant{ get; set; }
        /// <summary>
        /// Get or sets the device groups data model
        /// </summary>
        /// <summary>
        /// Relation between tables.
        /// </summary>
        /// <param name="modelBuilder">Entity framework model builder before creating database</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserLoginConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationRoleClaimConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserTokenConfiguration());

            modelBuilder.ApplyConfiguration(new TenantEntityConfiguration());
            modelBuilder.ConvertToSnakeCase();

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ChangeTracker.DetectChanges();
            ChangeTracker.ProcessModification(Guid.NewGuid());
            ChangeTracker.ProcessDeletion(Guid.NewGuid());
            ChangeTracker.ProcessCreation(Guid.NewGuid());
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
