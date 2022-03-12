using Core.Dto;
using Core.Dto.Security;
using Core.EF.Data;
using Core.EF.IdentityModels;
using Core.EF.Managers.IdentityExtensions;
using Core.Infrastructure.DataAccess;
using Core.Infrastructure.Pagination;
using Core.Infrastructure.Tenancy;
using Core.Security.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.EF.Managers
{
    public class ApplicationUserManager : IApplictionUserManager
    {
        #region Ctor & Properties
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPager _pager;
        private readonly IStringLocalizer<ApplicationUserManager> _localizer;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITenantProvider _tenantProvider;

        public ApplicationUserManager(UserManager<ApplicationUser> userManager, IPager pager,
                                      ITenantProvider tenantProvider,
                                      SignInManager<ApplicationUser> signInManager,
                                      IStringLocalizer<ApplicationUserManager> localizer)
        {
            _userManager = userManager;
            _pager = pager;
            _localizer = localizer;
            _signInManager = signInManager;
            _tenantProvider = tenantProvider;
        }
        #endregion


        #region "METHODS"
       
        public async Task<PaginatedResult<AppUser>> GetAllUsers(string searchText, Guid? roleId, int page, int pageSize)
        {
            IQueryable<ApplicationUser> users = _userManager.Users
                                                              .Include(e => e.ApplicationUserRoles)
                                                              .ThenInclude(e => e.ApplicationRole)
                                                              .Where(e => (string.IsNullOrEmpty(searchText)|| 
                                                                            e.FirstName.ToLower().Contains(searchText.ToLower())||
                                                                            e.LastName.ToLower().Contains(searchText.ToLower())|| 
                                                                            e.Email.ToLower().Contains(searchText.ToLower())|| 
                                                                            e.UserName.ToLower().Contains(searchText.ToLower())|| 
                                                                            e.PhoneNumber.ToLower().Contains(searchText.ToLower()))
                                                                            && e.TenantId == _tenantProvider.TenantId);

            if (roleId.HasValue)
                users = users.Where(e => e.ApplicationUserRoles.Select(e => e.RoleId).Contains(roleId.Value));

            return await _pager.ConvertToPagedListAsync(users.Select(Select), page, pageSize).ConfigureAwait(false);
        }


        public async Task<IEnumerable<AppUser>> GetManyAsync(List<Guid> ids)
        {
            return await _userManager.Users.Where(e => ids.Contains(e.Id)).Select(Select).ToListAsync();
        }


        public async Task<Result<AppUser>> FindByIdAsync(Guid id)
        {
            AppUser user = await _userManager.Users
                                       .Include(e => e.ApplicationUserRoles).ThenInclude(e => e.ApplicationRole)
                                       .Select(Select)
                                       .FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
            if (user == null)
                return await Result<AppUser>.FailAsync(_localizer["NotFound"]);
            return await Result<AppUser>.SuccessAsync(user);
        }

        public async Task<ApplicationIdentityResult> AddAsync(AppUser user)
        {
            ApplicationUser applicationUser = user.ToApplicationUser();
            applicationUser.Id = Guid.NewGuid();
            var result = await _userManager.CreateAsync(applicationUser).ConfigureAwait(false);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(applicationUser, user.Roles[0].Name);
            }
            return result.ToApplicationIdentityResult();
        }

        public async Task<Result<AppUser>> UpdateAsync(AppUser user)
        {
            var applicationUser = await _userManager.FindByIdAsync(user.Id.ToString()).ConfigureAwait(false);
            applicationUser.FirstName = user.FirstName;
            applicationUser.LastName = user.LastName;
            applicationUser.Email = user.Email;
            applicationUser.UserName = user.UserName;
            applicationUser.PhoneNumber = user.PhoneNumber;
            applicationUser.DOB = user.DOB;
            applicationUser.ImageUrl = user.ImageUrl;
            applicationUser.IsActive = user.IsActive;
            var result = await _userManager.UpdateAsync(applicationUser).ConfigureAwait(false);
            if (result.Succeeded)
                return await Result<AppUser>.SuccessAsync(user);
            return await Result<AppUser>.FailAsync("Something went wrong");
        }

        public async Task<bool> IsUserInTenantAsync(Guid userId, Guid tenantId)
        {
            return await _userManager.Users.AnyAsync(e => e.Id == userId && e.TenantId == tenantId).ConfigureAwait(false);
        }


        public async Task<Result> DeleteAsync(AppUser appUser)
        {
            var result = await _userManager.DeleteAsync(appUser.ToApplicationUser()).ConfigureAwait(false);
            return result.Succeeded ?
             await Result<AppUser>.SuccessAsync(_localizer["User.Deleted.Successfully"]) :
                   await Result<AppUser>.FailAsync(_localizer["User.SomethingWentWrong"]);
        }

        public async Task<IEnumerable<DropDownDto<Guid>>> GetUserDropDownAsync(string searchText)
        {
            searchText = string.IsNullOrEmpty(searchText) ? "" : searchText.Trim().ToLower();
            return await _userManager.Users
                                     .Where(e => string.IsNullOrWhiteSpace(searchText) || e.FirstName.ToLower().Contains(searchText)
                                                                                       || e.LastName.ToLower().Contains(searchText))
                                     .Select(e => new DropDownDto<Guid>(e.Id, $"{e.FirstName} {e.LastName}"))
                                     .ToListAsync()
                                     .ConfigureAwait(false);
        }

        public async Task<Result> DoesUsernameAlreadyExistsAsync(string userName)
        {
            var isExists = await _userManager.Users.AnyAsync(e => e.UserName.ToLower() == userName.ToLower()).ConfigureAwait(false);
            return isExists ?
                await Result.FailAsync() as Result : await Result.SuccessAsync() as Result;
        }

        public async Task<Result> DoesPhoneAlreadyExistsAsync(string phone)
        {
            var isExists = await _userManager.Users.AnyAsync(e => e.PhoneNumber == phone).ConfigureAwait(false);
            return isExists ?
                await Result.FailAsync() as Result : await Result.SuccessAsync() as Result;
        }

        public async Task<bool> PasswordSignInAsync(string userName, string password)
        {
            var user=await _userManager.FindByNameAsync(userName);
            var result = await _signInManager.PasswordSignInAsync(user,password,false,false);
            return result.Succeeded;
        }

        public async Task<AppUser> FindByEmailAsync(string email)
        {
            var user= await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            return user.ToAppUser();
        }

        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            var applicationUser=await _userManager.FindByNameAsync(user.UserName);
            return await _userManager.CheckPasswordAsync(applicationUser,password);
        }

        public Task<IList<string>> GetRolesAsync(AppUser user)
        {
            var applicationUser = user.ToApplicationUser();
            var roles=_userManager.GetRolesAsync(applicationUser);
            return roles;
        }
        public Task<IList<Claim>> GetClaimsAsync(AppUser user)
        {
            var applicationUser = user.ToApplicationUser();
            var roles = _userManager.GetClaimsAsync(applicationUser);
            return roles;
        }

        public IQueryable<AppUser> GetAll()
        {
            return _userManager.Users.Select(e=>e.ToAppUser()).AsQueryable();
        }

        #endregion

        #region "User Selector expression"
        private static readonly Expression<Func<ApplicationUser, AppUser>> Select = e => new AppUser
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            UserName = e.UserName,
            ImageUrl = e.ImageUrl,
            DOB = e.DOB,
            BranchId = e.BranchId,
            Roles = e.ApplicationUserRoles.Select(e => e.ApplicationRole)
                                          .Select(e => new KeyValue(e.Id, e.Name))
                                          .ToList(),
        };
        #endregion
    }

    public class DTOO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
