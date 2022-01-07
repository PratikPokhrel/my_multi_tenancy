using Core.Dto;
using Core.Dto.Security;
using Core.EF.IdentityModels;
using Core.Infrastructure;
using Core.Security.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.EF.Managers
{
    public class RoleClaimManager : IRoleClaimManager
    {
        #region "Ctor & properties"
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleClaimManager(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
        #endregion
        public async Task<Result> AddAsync(AppRoleClaim appRoleClaim)
        {
            ApplicationRole role = await _roleManager.FindByIdAsync(appRoleClaim.RoleId.ToString());
            if (role == null)
                return (Result)await Result.FailAsync("Role does not exists").ConfigureAwait(false);
            await _roleManager.AddClaimAsync(role, new Claim(appRoleClaim.ClaimValue, appRoleClaim.ClaimType));
            return (Result)await Result.SuccessAsync("Role added successfully").ConfigureAwait(false);
        }
    }
}
