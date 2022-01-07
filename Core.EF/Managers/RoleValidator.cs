using Core.EF.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.EF.Managers
{
    public class MyRoleValidator : RoleValidator<ApplicationRole>
    {
        private IdentityErrorDescriber Describer { get; set; }

        public MyRoleValidator() : base()
        {

        }
        public override async Task<IdentityResult> ValidateAsync(RoleManager<ApplicationRole> manager, ApplicationRole role)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            var errors = new List<IdentityError>();
            await ValidateRoleName(manager, role, errors);
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors[0]);
            }
            return IdentityResult.Success;
        }
        private async Task ValidateRoleName(RoleManager<ApplicationRole> manager, ApplicationRole role,
        ICollection<IdentityError> errors)
        {
            var roleName = await manager.GetRoleNameAsync(role);
            if (string.IsNullOrWhiteSpace(roleName))
            {
                errors.Add(Describer.InvalidRoleName(roleName));
            }
            else
            {
                var owner = await manager.FindByNameAsync(roleName);
                if (owner != null
                    && owner.TenantId == role.TenantId
                    && !string.Equals(await manager.GetRoleIdAsync(owner), await manager.GetRoleIdAsync(role)))
                {
                    errors.Add(Describer.DuplicateRoleName(roleName));
                }
            }
        }
    }
}
