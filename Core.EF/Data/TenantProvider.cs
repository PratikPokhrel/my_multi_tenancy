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
   public class TenantProvider:ITenantProvider
    {
        private readonly DefaultContext _defaultContext;
        private readonly string _host;

        public TenantProvider(IHttpContextAccessor accessor,DefaultContext defaultContext)
        {
            _defaultContext = defaultContext;
            _host = accessor.HttpContext.Request.Host.ToString();
        }

        public Tenant GetTenant()
        {
            return _defaultContext.Tenant
                    .Where(t => t.Identifier.ToLower() == _host.ToLower())
                    .FirstOrDefault();
        }
    }
}
