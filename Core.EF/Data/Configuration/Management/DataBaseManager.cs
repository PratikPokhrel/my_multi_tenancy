using System;
using System.Collections.Generic;
using System.Linq;
using Core.Constants;
using Core.EF.Data.Context;
using Core.EF.Data.Context.Default;
using Core.Entities;

namespace Core.EF.Data.Configuration.Management
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
        private readonly DefaultContext _defaultContext;

        /// <summary>
        /// Gets the name of the data base.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>db name</returns>
        public string GetDataBaseName(string tenantId)
        {
            var dataBaseName = tenantConfigurationDictionary[Guid.Parse(tenantId)];

            if (dataBaseName == null)
            {
                throw new ArgumentNullException(nameof(dataBaseName));
            }

            return dataBaseName;
        }


        public DataBaseManager(DefaultContext defaultContext)
        {
            _defaultContext = defaultContext;
        }
        public Tenant GetTenant(string tenantId)
        {
            return _defaultContext.Tenant.FirstOrDefault(e => e.Id == Guid.Parse(tenantId));
        }

    }
}