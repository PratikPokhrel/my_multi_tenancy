using Core.Dto;
using Core.Dto.Security;
using Core.EF.IdentityModels;
using Core.EF.Managers.IdentityExtensions;
using Core.Infrastructure;
using Core.Infrastructure.Tenancy;
using Core.Security.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.EF.Managers
{
    public class RoleManager: IRoleManager
    {
        #region Ctor & Properties
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleManager(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
        #endregion



        #region "Methods"

        public async Task<AppRole> FindByIdAsync(Guid id)
        {
            var role=await _roleManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
            if (role == null)
                return null;
            return role.ToAppRole();
        }

        public async Task<AppRole> FindByNameAsync(string name)
        {
            var role = await _roleManager.FindByNameAsync(name).ConfigureAwait(false);
            return role.ToAppRole();
        }

        public async Task<ApplicationIdentityResult> DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
            if (role == null) return null;

            IdentityResult result=await _roleManager.DeleteAsync(role).ConfigureAwait(false);
            return result.ToApplicationIdentityResult();
        }

        public async Task<IEnumerable<AppRole>> GetAllAsync(string search, bool loadClaims)
        {
           IEnumerable<ApplicationRole> roles=await _roleManager.Roles
                                                                .Where(e=>(string.IsNullOrEmpty(search) || 
                                                                      e.Name.ToLower().Contains(search.ToLower())))
                                                                .OrderBy(e=>e.Name)
                                                                .ToListAsync()
                                                                .ConfigureAwait(false);
            if (loadClaims)
                foreach (ApplicationRole role in roles)
                    role.ApplicationRoleClaims = await GetRoleClaimsAsync(role).ConfigureAwait(false);

            return roles.ToAppRole();
        }

        public async Task<Result> CreateAsync(AppRole role)
        {
            var applicationRole = new ApplicationRole(role.Name);
            applicationRole.Id = Guid.NewGuid();
            var result = await _roleManager.CreateAsync(applicationRole).ConfigureAwait(false);
            if (result.Succeeded)
                return (Result)await Result.SuccessAsync("");

            return (Result)await Result.FailAsync(result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<Result> UpdateAsync(AppRole role)
        {
            var applicationRole = new ApplicationRole(role.Id,role.Name);
            await _roleManager.UpdateAsync(applicationRole).ConfigureAwait(false);
            return  (Result)await Result.SuccessAsync();
        }

        public async Task<bool> ExistsAsync(string name,Guid? id = null)
        {
            return await _roleManager.Roles
                                              .AnyAsync(e => e.Name.ToLower().Equals(name.ToLower())
                                                         && (!id.HasValue || e.Id != id)).ConfigureAwait(false);
        }

        public async Task<List<ApplicationRoleClaim>> GetRoleClaimsAsync(Guid roleId)
        {
           var role=await _roleManager.FindByIdAsync(roleId.ToString()).ConfigureAwait(false);
           return await GetRoleClaimsAsync(role).ConfigureAwait(false);
        }

        public async Task<List<ApplicationRoleClaim>> GetRoleClaimsAsync(ApplicationRole role)
        {
            var claims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
            return claims.Select(c => new ApplicationRoleClaim { ClaimType = c.Type, ClaimValue = c.Value }).ToList();
        }

        public Task<IList<Claim>> GetClaimsAsync(AppRole role)
        {
            var applicationRole = role.ToApplicationRole();
            var roles = _roleManager.GetClaimsAsync(applicationRole);
            return roles;
        }
        #endregion
    }
}
