using System;
using DeviceManager.Api.Configuration.DatabaseTypes;
using DeviceManager.Api.Configuration.Settings;
using DeviceManager.Api.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using my_multi_tenancy.Data.Context;

namespace DeviceManager.Api.Data.Management
{
    /// <summary>
    /// Entity Framework context service
    /// (Switches the db context according to tenant id field)
    /// </summary>
    /// <seealso cref="IContextFactory"/>
    public class ContextFactory : IContextFactory
    {
        private const string TenantIdFieldName = DefaultConstants.TenantId;
        private const string DatabaseFieldKeyword = DefaultConstants.Database;
        private readonly HttpContext httpContext;

        private readonly ConnectionSettings connectionOptions;

        private readonly IDataBaseManager dataBaseManager;
        private readonly IDatabaseType databaseType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFactory"/> class.
        /// </summary>
        /// <param name="httpContentAccessor">The HTTP content accessor.</param>
        /// <param name="connectionOptions">The connection options.</param>
        /// <param name="dataBaseManager">The data base manager.</param>
        /// <param name="databaseType">Type of the database</param>
        /// <param name="dataSeeder">Data seeder</param>
        public ContextFactory(IHttpContextAccessor httpContentAccessor,
            ConnectionSettings connectionOptions,
            IDataBaseManager dataBaseManager,
            IDatabaseType databaseType)
        {
            this.httpContext = httpContentAccessor.HttpContext;
            this.connectionOptions = connectionOptions;
            this.dataBaseManager = dataBaseManager;
            this.databaseType = databaseType;
        }

        /// <inheritdoc />
        public IDbContext DbContext => ChangeDatabaseNameInConnectionString(this.TenantId);

        /// <summary>
        /// Gets tenant id from HTTP header
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// httpContext
        /// or
        /// tenantId
        /// </exception>
        private string TenantId
        {
            get
            {
                ValidateHttpContext();

                string tenantId = this.httpContext.Request.Headers[TenantIdFieldName].ToString();
                tenantId=Guid.NewGuid().ToString(); 

                ValidateTenantId(tenantId);

                return tenantId;
            }
        }

        private IDbContext ChangeDatabaseNameInConnectionString(string tenantId)
        {
            ValidateDefaultConnection();

            // 1. Create Connection String Builder using Default connection string
            var connectionBuilder = databaseType.GetConnectionBuilder(connectionOptions.DefaultConnection);

            // 2. Remove old Database info from connection string
            connectionBuilder.Remove(DatabaseFieldKeyword);
            connectionBuilder.Remove(DefaultConstants.DbServer);
            connectionBuilder.Remove(DefaultConstants.DbUser);
            connectionBuilder.Remove(DefaultConstants.DbPassword);

            var tenant=GetTenant(tenantId);
            if (tenant.DatabaseType == (int)DatabaseType.MsSql)
            {
                connectionBuilder.Add(DatabaseFieldKeyword, tenant.Database);
                connectionBuilder.Add(DefaultConstants.DbServer, tenant.Server);
                connectionBuilder.Add(DefaultConstants.DbUser, tenant.User);
                connectionBuilder.Add(DefaultConstants.DbPassword, tenant.Password);
                // 4. Create DbContextOptionsBuilder with new Database name
                var contextOptionsBuilder = new DbContextOptionsBuilder<AccountContext>();
                databaseType.SetConnectionString(contextOptionsBuilder, connectionBuilder.ConnectionString);
                return new AccountContext(contextOptionsBuilder.Options);
            }
            else
            {
                var contextOptionsBuilder = new DbContextOptionsBuilder<AccountContext>();
                databaseType.SetConnectionString(contextOptionsBuilder, connectionBuilder.ConnectionString);
                return new AccountContext(contextOptionsBuilder.Options);
            }
        }

        public static Tenant GetTenant(string tenantId)
        {
            return new Tenant
            {
                Server = "3.109.16.202",
                Database = "accounts-dev-devbranch",
                User = "erp",
                Password = "h+&xQGP=JEaQ4Nsy",
                DatabaseType =0
            };
        }

        private void ValidateDefaultConnection()
        {
            if (string.IsNullOrEmpty(this.connectionOptions.DefaultConnection))
            {
                throw new ArgumentNullException(nameof(this.connectionOptions.DefaultConnection));
            }
        }

        private void ValidateHttpContext()
        {
            if (this.httpContext == null)
            {
                throw new ArgumentNullException(nameof(this.httpContext));
            }
        }

        private static void ValidateTenantId(string tenantId)
        {
            if (tenantId == null)
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (!Guid.TryParse(tenantId, out Guid tenantGuid))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }
        }
    }
}
