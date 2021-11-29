using System;
using System.Data.Common;
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
        private readonly string tenant1 = "3249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1";
        private readonly string tenant2 = "4249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1";
        private readonly string tenant3 = "5249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFactory"/> class.
        /// </summary>
        /// <param name="httpContentAccessor">The HTTP content accessor.</param>
        /// <param name="connectionOptions">The connection options.</param>
        /// <param name="dataBaseManager">The data base manager.</param>
        public ContextFactory(IHttpContextAccessor httpContentAccessor,
            ConnectionSettings connectionOptions,
            IDataBaseManager dataBaseManager)
        {
            this.httpContext = httpContentAccessor.HttpContext;
            this.connectionOptions = connectionOptions;
            this.dataBaseManager = dataBaseManager;
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
                ValidateTenantId(tenantId);

                return tenantId;
            }
        }

        private IDbContext ChangeDatabaseNameInConnectionString(string tenantId)
        {
            ValidateDefaultConnection();

            var tenant = GetTenant(tenantId);

            IDatabaseType dbType =tenant.DatabaseType==(int)DatabaseType.MsSql? new MsSql():new Postgres();
            // 1. Create Connection String Builder using Default connection string

            DbConnectionStringBuilder connectionBuilder = dbType.GetConnectionBuilder(tenant.DatabaseType == (int)DatabaseType.MsSql
                                                                ? DefaultConstants.MsSqlConnectionStringFormat 
                                                                : connectionOptions.DefaultConnection);


            // 2. Remove old Database info from connection string
            connectionBuilder.Clear();

            if (tenant.DatabaseType == (int)DatabaseType.MsSql)
            {
                connectionBuilder.Add(DatabaseFieldKeyword, tenant.Database);
                connectionBuilder.Add(DefaultConstants.Password, tenant.Password.Trim());
                connectionBuilder.Add(DefaultConstants.DbServer, tenant.Server);
                connectionBuilder.Add(DefaultConstants.DbUser, tenant.User);


                // 4. Create DbContextOptionsBuilder with new Database name
                var contextOptionsBuilder = new DbContextOptionsBuilder<AccountContext>();
                dbType.SetConnectionString(contextOptionsBuilder, connectionBuilder.ConnectionString);
                return new AccountContext(contextOptionsBuilder.Options);
            }
            else if (tenant.DatabaseType == (int)DatabaseType.Postgres)
            {
                connectionBuilder.Add(DatabaseFieldKeyword, tenant.Database);
                connectionBuilder.Add(DefaultConstants.DbUserID, tenant.User);
                connectionBuilder.Add(DefaultConstants.Password, tenant.Password);
                connectionBuilder.Add(DefaultConstants.DbHost, tenant.Server);
                connectionBuilder.Add(DefaultConstants.DbPort, tenant.Port);


                // 4. Create DbContextOptionsBuilder with new Database name
                var contextOptionsBuilder = new DbContextOptionsBuilder<DeviceContext>();
                dbType.SetConnectionString(contextOptionsBuilder, connectionBuilder.ConnectionString);
                return new DeviceContext(contextOptionsBuilder.Options);
            }
            else
                throw new ArgumentNullException("Database Type");
        }

        public Tenant GetTenant(string tenantId)
        {
            if (tenantId == tenant1)
            {
                return new Tenant
                {
                    Server = "3.109.16.202",
                    Database = "accounts-dev-devbranch",
                    User = "erp",
                    Password = "h+&xQGP=JEaQ4Nsy",
                    DatabaseType = 0
                };
            }
            else if(tenantId == tenant2)
                return new Tenant
                {
                    Server = "3.109.16.202",
                    Database = "accounts-dev-daraz",
                    User = "erp",
                    Password = "h+&xQGP=JEaQ4Nsy",
                    DatabaseType = 0
                };
            else
                return new Tenant
                {
                    Server = "localhost",
                    Database = "movie_db",
                    User = "postgres",
                    Password = "Admin",
                    Port= "5432",
                    DatabaseType = 1
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
