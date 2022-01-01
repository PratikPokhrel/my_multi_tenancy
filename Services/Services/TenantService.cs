using Core.Entities;
using Core.Infrastructure.DataAccess;
using Core.Infrastructure.Tenancy;
using Core.Security.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TenantService : ITenantService
    {
        private readonly IDefaultUnitOfWork _uow;
        private readonly IApplictionUserManager _userManager;
        private readonly ITenantProvider _tenantProvider;

        public TenantService(IDefaultUnitOfWork uow,IApplictionUserManager userManager,ITenantProvider tenantProvider)
        {
            _uow = uow;
            _userManager = userManager;
            _tenantProvider = tenantProvider;
        }
        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
          return  await _uow.GetRepository<Tenant>()
                                          .GetAllAsync(orderBy:e=>e.OrderBy(x=>x.Server)).ConfigureAwait(false);
        }

        public async Task<bool> IsUserInTenantAsync(Guid userId)
        {
          return await  _userManager.IsUserInTenantAsync(userId,_tenantProvider.TenantId).ConfigureAwait(false);
        }
    }
}
