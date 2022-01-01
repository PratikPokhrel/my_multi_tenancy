using Core.EF.Data.Context.Default;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Tenancy
{
    public class TenantProvider : ITenantProvider
    {
        private readonly ITenantSource _tenantSource;
        private readonly string _host;
        private static Tenant _tenant;

        public TenantProvider(IHttpContextAccessor accessor, ITenantSource tenantSource)
        {
            _tenantSource = tenantSource;
            _host = accessor.HttpContext.Request.Host.ToString();
            _tenant = GetCurrentTenant(_host, _tenantSource);
        }
        private static Tenant GetCurrentTenant(string _host, ITenantSource _tenantSource)
        {
            return _tenantSource.ListTenants()
                                 .Where(t => t.Identifier.ToLower() == _host.ToLower())
                                 .FirstOrDefault();
        }

        public Guid TenantId => _tenant.Id;
        public Tenant Tenant => _tenant;
    }
}
