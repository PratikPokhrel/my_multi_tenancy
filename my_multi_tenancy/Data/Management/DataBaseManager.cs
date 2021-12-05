using System;
using System.Collections.Generic;
using System.Linq;
using DeviceManager.Api.Constants;
using my_multi_tenancy.Data.Context;

namespace DeviceManager.Api.Data.Management
{
    /// <summary>
    /// Contains all tenants database mappings and options
    /// </summary>
    public class DataBaseManager : IDataBaseManager
    {
        /// <summary>
        /// IMPORTANT NOTICE: Tenant Configuration was implemented as Dictionary for demo purposes only 
        /// In a production application I would recommend following options:
        /// - create SQL root database or table
        /// - create NoSql root database/collection
        /// - move the dictionary the Redis cache  
        /// </summary>
        private readonly Dictionary<Guid, string> tenantConfigurationDictionary = new Dictionary<Guid, string>
        {
            {
                Guid.Parse(DefaultConstants.DefaultTenantGuid), DefaultConstants.DefaultTeanantDatabase
            },
            {
                Guid.Parse(DefaultConstants.Tenant2Guid), DefaultConstants.DeviceDbTenant2
            }
        };

        /// <summary>
        /// Gets the name of the data base.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>db name</returns>
        public string GetDataBaseName(string tenantId)
        {
            var dataBaseName = this.tenantConfigurationDictionary[Guid.Parse(tenantId)];

            if (dataBaseName == null)
            {
                throw new ArgumentNullException(nameof(dataBaseName));
            }

            return dataBaseName;
        }

        public Tenant GetTenant(string tenantId)
        {
            return tenants.FirstOrDefault(e=>e.Id==Guid.Parse(tenantId));
        }

        List<Tenant> tenants = new List<Tenant>
        {
            new Tenant
                {
                   Id=Guid.Parse("3249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"),
                    Server = "3.109.16.202",
                    Database = "accounts-dev-devbranch",
                    User = "erp",
                    Password = "h+&xQGP=JEaQ4Nsy",
                    DatabaseType = 0
                },
            new Tenant
                {
                   Id=Guid.Parse("4249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"),
                    Server = "3.109.16.202",
                    Database = "accounts-dev-daraz",
                    User = "erp",
                    Password = "h+&xQGP=JEaQ4Nsy",
                    DatabaseType = 0
                },
            new Tenant
                {
                   Id=Guid.Parse("5249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"),
                    Server = "localhost",
                    Database = "movie_db",
                    User = "postgres",
                    Password = "Admin",
                    Port= "5432",
                    DatabaseType = 1
                }
        };
    }
}