using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Tenancy
{
    public interface ITenantProvider
    {
        /// <summary>
        /// Current Tenant
        /// </summary>
        Tenant Tenant { get; }

        /// <summary>
        /// Current Tenant Id
        /// </summary>
        Guid TenantId { get; }
    }
}
