﻿using System;
using System.Data.Common;
using Core.Constants;
using Core.EF.Data.Configuration.DatabaseTypes;
using Core.EF.Data.Configuration.Pg;
using Core.EF.Data.Context;
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
        private const string TenantIdFieldName = DefaultConstants.TenantId;
        private const string DatabaseFieldKeyword = DefaultConstants.Database;
        private readonly HttpContext httpContext;
        private readonly ConnectionSettings connectionOptions;
        private readonly ITenantProvider _tenantProvider;
        private readonly IDataBaseManager dataBaseManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFactory"/> class.
        /// </summary>
        /// <param name="httpContentAccessor">The HTTP content accessor.</param>
        /// <param name="connectionOptions">The connection options.</param>
        /// <param name="dataBaseManager">The data base manager.</param>
        public ContextFactory(IHttpContextAccessor httpContentAccessor,ITenantProvider tenantProvider, ConnectionSettings connectionOptions, IDataBaseManager dataBaseManager)
        {
            httpContext = httpContentAccessor.HttpContext;
            this.connectionOptions = connectionOptions;
            _tenantProvider = tenantProvider;
            this.dataBaseManager = dataBaseManager;
        }

        /// <inheritdoc />
        public IDbContext DbContext => ChangeDatabaseNameInConnectionString(Tenant);

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
        private Tenant Tenant
        {
            get
            {
                ValidateHttpContext();
                Tenant tenant= _tenantProvider.GetTenant();
                return tenant ?? throw new ArgumentNullException("Not a valid tenant");
            }
        }

        private IDbContext ChangeDatabaseNameInConnectionString(Tenant tenant)
        {
            //ValidateDefaultConnection();


            IDatabaseType dbType = tenant.DatabaseType == (int)DatabaseType.MsSql ? new MsSql() : new Postgres();
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