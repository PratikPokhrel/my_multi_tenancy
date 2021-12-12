using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ITenantService
    {
        Task<IEnumerable<Tenant>> GetAllAsync();
        bool IsUserInTenant(Guid userId);
    }
}
