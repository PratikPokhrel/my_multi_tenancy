using Core.EF.Configs.EntityConfigs.Security;
using Core.EF.Configs.EntityConfigs.Tenants;
using Core.EF.Data.Extensions;
using Core.EF.IdentityModels;
using Core.Entities;
using Core.Entities.Audit;
using Core.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.EF.Data.Context.Default
{
    public class DefaultContext:IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>,IDbContext
    {
        private readonly ITenantProvider _tenantProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="dataSeeder">Initial data seeder</param>
        public DefaultContext(DbContextOptions<DefaultContext> options,ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
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
            modelBuilder.ConfigureSecurityEntities();
            modelBuilder.ApplyConfiguration(new TenantEntityConfiguration());
            modelBuilder.ApplyGlobalFilters<IHasTenant>(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.ConvertToSnakeCase();

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ChangeTracker.DetectChanges();
            ChangeTracker.ProcessModification(Guid.NewGuid());
            ChangeTracker.ProcessDeletion(Guid.NewGuid());
            ChangeTracker.ProcessCreation(Guid.NewGuid(), _tenantProvider.TenantId);
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
