using Core.Dto.Security;
using Core.EF.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EF.Managers.IdentityExtensions
{
    public static class IdentityExtensions
    {
        public static AppRole ToAppRole(this ApplicationRole applicationRole)
        {
            if (applicationRole == null) return null;
            return new AppRole
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name,
                //RoleClaims=applicationRole.ApplicationRoleClaims.Select(e=>new AppRoleClaim { ClaimType = e.ClaimType,ClaimValue = e.ClaimValue}).ToList()
            };
        }

        public static IEnumerable<AppRole> ToAppRole(this IEnumerable<ApplicationRole> applicationRoles)
        {
            return applicationRoles.Select(e =>e.ToAppRole());
        }

        public static ApplicationIdentityResult ToApplicationIdentityResult(this IdentityResult identityResult)
        {
            return identityResult == null ? null : new ApplicationIdentityResult(identityResult.Errors.Select(x => x.Code + x.Description).ToList(), identityResult.Succeeded);
        }

        public static ApplicationUser ToApplicationUser(this AppUser appUser)
        {
            if(appUser == null) return null;
            return new ApplicationUser
            {
                Id=appUser.Id!=Guid.Empty?appUser.Id:Guid.Empty,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                UserName = appUser.UserName,
                PhoneNumber=appUser.PhoneNumber,
                DOB=appUser.DOB,
            };
        }
        public static AppUser ToAppUser(this ApplicationUser applicationUser)
        {
            if (applicationUser == null) return null;
            return new AppUser
            {
                Id = applicationUser.Id != Guid.Empty ? applicationUser.Id : Guid.Empty,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Email = applicationUser.Email,
                UserName = applicationUser.UserName,
                PhoneNumber = applicationUser.PhoneNumber,
                DOB = applicationUser.DOB,
                IsActive=applicationUser.IsActive
            };
        }

        public static ApplicationRole ToApplicationRole(this AppRole appRole)
        {
            if (appRole == null) return null;
            return new ApplicationRole
            {
                Id = appRole.Id != Guid.Empty ? appRole.Id : Guid.Empty,
                Name = appRole.Name,
            };
        }

    }
}
