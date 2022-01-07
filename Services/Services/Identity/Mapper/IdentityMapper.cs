using Core.Dto.Security;
using Services.Services.Dtos.Resp.Identity;
using Services.Services.Dtos.Rqst.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Identity.Mapper
{
    internal static class IdentityMapper
    {
        public static RoleResp? ToResp(this AppRole appRole)
        {
            if (appRole == null) 
                return null;

            return new RoleResp
            {
                Id = appRole.Id,
                Name = appRole.Name,
                Claims=appRole.RoleClaims.Select(e=>new RoleClaimResp { Type=e.ClaimType, Value=e.ClaimValue }).ToList()
            };
        }

        public static IEnumerable<RoleResp> ToResp(this IEnumerable<AppRole> appRoles)
        {
            return appRoles.Select(ToResp);
        }

        public static AppRole ToAppRole(this RoleRqst rqst)
        {
            if (rqst == null)
                return null;

            return new AppRole
            {
                Id = rqst.Id,
                Name = rqst.Name,
            };
        }

        public static RoleResp ToResp(this RoleRqst rqst)
        {
            if (rqst == null)
                return null;

            return new RoleResp
            {
                Id = rqst.Id,
                Name = rqst.Name,
            };
        }
    }
}
