using Core.Entities;
using Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TenantService : ITenantService
    {
        private readonly IDefaultUnitOfWork _defaultUnitOfWork;

        public TenantService(IDefaultUnitOfWork defaultUnitOfWork)
        {
            _defaultUnitOfWork = defaultUnitOfWork;
        }
        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
          return  await _defaultUnitOfWork.GetRepository<Tenant>()
                                          .GetAllAsync(orderBy:e=>e.OrderBy(x=>x.Server)).ConfigureAwait(false);
        }

        public bool IsUserInTenant(Guid userId)
        {
            return false;
        }
    }
}
