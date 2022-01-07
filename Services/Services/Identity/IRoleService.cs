using Core.Dto;
using Core.Dto.Security;
using Core.Infrastructure.Pagination;
using Services.Services.Dtos.Resp.Identity;
using Services.Services.Dtos.Rqst.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Identity
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResp>> GetAllAsync(string name,bool loadClaims);
        Task<Result<RoleResp>> CreateAsync(RoleRqst role);
        Task<Result<RoleResp>> UpdateAsync(RoleRqst role);
        Task<Result<RoleResp>> DeleteAsync(Guid id);
        Task<Result<RoleResp>> GetAsync(Guid id);
        Task<Result> AddClaimAsync(AppRoleClaim roleClaim);
    }
}
