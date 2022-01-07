using Core.Entities;
using Core.Infrastructure.Tenancy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class FileTenantSource: ITenantSource
    {
        public Tenant[] ListTenants()
        {
            var tenants = File.ReadAllText("tenants.json");
            return JsonConvert.DeserializeObject<Tenant[]>(tenants);
        }
    }
}
