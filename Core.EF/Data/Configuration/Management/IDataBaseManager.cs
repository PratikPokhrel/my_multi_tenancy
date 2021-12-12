using Core.EF.Data.Context;
using Core.Entities;

namespace Core.EF.Data.Configuration.Management
{
    /// <summary>
    /// Multitenant database manager
    /// </summary>
    public interface IDataBaseManager
    {
        /// <summary>
        /// Gets the name of the data base.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>db name</returns>
        string GetDataBaseName(string tenantId);

        Tenant GetTenant(string tenantId);
    }
}