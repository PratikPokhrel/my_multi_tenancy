using System;
using System.Data.Common;
using Core.Constants;
using Core.EF.Data.Configuration.DatabaseTypes;
using Core.EF.Data.Configuration.Pg;
using Core.EF.Data.Context;
using Core.EF.Data.Extensions;
using Core.Entities;
using Core.Infrastructure.Tenancy;
using DeviceManager.Api.Configuration.DatabaseTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core.EF.Data.Configuration.Management
{
    /// <summary>
    /// Entity Framework context service
    /// (Switches the db context according to tenant id field)
    /// </summary>
    /// <seealso cref="IContextFactory"/>
    public class ContextFactory : IContextFactory
    {
        private readonly HttpContext httpContext;
        private readonly ConnectionSettings connectionOptions;
        private readonly ITenantProvider _tenantProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFactory"/> class.
        /// </summary>
        /// <param name="httpContentAccessor">The HTTP content accessor.</param>
        /// <param name="connectionOptions">The connection options.</param>
        /// <param name="dataBaseManager">The data base manager.</param>
        public ContextFactory(IHttpContextAccessor httpContentAccessor, ITenantProvider tenantProvider, ConnectionSettings connectionOptions)
        {
            httpContext = httpContentAccessor.HttpContext;
            this.connectionOptions = connectionOptions;
            _tenantProvider = tenantProvider;
        }

        /// <inheritdoc />
        public IDbContext DbContext => ChangeDbContext(Tenant);

        /// <summary>
        /// Gets Current Tenant
        /// </summary>
        private Tenant Tenant
        {
            get
            {
                ValidateHttpContext();
                Tenant tenant = _tenantProvider.Tenant;
                return tenant ?? throw new ArgumentNullException("Not a valid tenant");
            }
        }

        private static IDbContext ChangeDbContext(Tenant tenant)
        {
            //ValidateDefaultConnection();


            IDatabaseType dbType = tenant.DatabaseType == (int)DatabaseType.MsSql ? new MsSql() : new Postgres();

            // 2. Remove old Database info from connection string

            if (tenant.DatabaseType == (int)DatabaseType.MsSql)
            {
                DbConnectionStringBuilder connectionBuilder = tenant.BuildConnectionString();
                // 4. Create MSSQL DbContextOptionsBuilder with new Database name
                var contextOptionsBuilder = new DbContextOptionsBuilder<DeviceContext>();
                dbType.SetConnectionString(contextOptionsBuilder, connectionBuilder.ConnectionString);
                return new DeviceContext(contextOptionsBuilder.Options);
            }
            else if (tenant.DatabaseType == (int)DatabaseType.Postgres)
            {
                DbConnectionStringBuilder connectionBuilder = tenant.BuildConnectionString();
                // 4. Create PSQL DbContextOptionsBuilder with new Database name
                var contextOptionsBuilder = new DbContextOptionsBuilder<DeviceContext>();
                dbType.SetConnectionString(contextOptionsBuilder, connectionBuilder.ConnectionString);
                return new DeviceContext(contextOptionsBuilder.Options);
            }
            else
            {
                ArgumentNullException argumentNullException = new("Database Type");
                throw argumentNullException;
            }
        }

        private void ValidateDefaultConnection()
        {
            if (string.IsNullOrEmpty(connectionOptions.DefaultConnection))
                throw new ArgumentNullException(nameof(connectionOptions.DefaultConnection));
        }

        private void ValidateHttpContext()
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));
        }

        private static void ValidateTenantId(string tenantId)
        {
            if (tenantId == null)
                throw new ArgumentNullException(nameof(tenantId));

            if (!Guid.TryParse(tenantId, out Guid tenantGuid))
                throw new ArgumentNullException(nameof(tenantId));
        }
    }
}
