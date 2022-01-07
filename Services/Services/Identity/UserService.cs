using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dto;
using Core.Dto.Security;
using Core.Entities;
using Core.Infrastructure;
using Core.Infrastructure.Pagination;
using Core.Security.User;
using Microsoft.Extensions.Localization;

namespace Services.Services.Identity
{
    public class UserService : IUserService
    {
        #region "Ctor & Properties"

        private readonly IApplictionUserManager _applictionUserManager;
        private readonly IBranchService _branchService;
        private readonly IStringLocalizer<UserService> _localizer;

        public UserService(IApplictionUserManager applictionUserManager, IBranchService branchService,
            IStringLocalizer<UserService> localizer)
        {
            _applictionUserManager = applictionUserManager;
            _branchService = branchService;
            _localizer = localizer;
        }

        #endregion

        public async Task<PaginatedResult<AppUser>> GetAllUsers(string searchText, Guid? roleId, int page, int pageSize)
        {
            PaginatedResult<AppUser> response = await _applictionUserManager
                .GetAllUsers(searchText, roleId, page, pageSize).ConfigureAwait(false);
            await LoadUserBranches(response.Items).ConfigureAwait(false);
            return response;
        }


        public async Task<Result<AppUser>> AddAsync(AppUser user)
        {
            var validate = await ValidateAddEditReqAsync(user);
            if (!validate.Succeeded)
                return validate;

            var result = await _applictionUserManager.AddAsync(user);
            return !result.Succeeded
                ? await Result<AppUser>.FailAsync(result.Errors)
                : await Result<AppUser>.SuccessAsync(user, string.Format(_localizer["User.Added.Successfully"],user.FullName));
        }

        public async Task<Result<AppUser>> UpdateAsync(AppUser appUser)
        {
            var validate = await ValidateAddEditReqAsync(appUser);
            if (!validate.Succeeded)
                return validate;

            var serviceResult = await _applictionUserManager.FindByIdAsync(appUser.Id).ConfigureAwait(false);
            if (!serviceResult.Succeeded)
                return serviceResult;

            return await _applictionUserManager.UpdateAsync(appUser).ConfigureAwait(false);
        }


        private async Task<Result<AppUser>> ValidateAddEditReqAsync(AppUser appUser)
        {
            if (appUser.Roles.Count == 0)
                return await Result<AppUser>.FailAsync(_localizer["User.Role.NotSelected"]);

            var result = await _applictionUserManager.DoesUsernameAlreadyExistsAsync(appUser.UserName)
                                                     .ConfigureAwait(false);

            if (!result.Succeeded)
                return await Result<AppUser>.FailAsync(_localizer["User.Username.AlreadyExists"]);

            if (!string.IsNullOrEmpty(appUser.PhoneNumber))
            {
                result = await _applictionUserManager.DoesPhoneAlreadyExistsAsync(appUser.UserName)
                                                     .ConfigureAwait(false);
                if (!result.Succeeded)
                    return await Result<AppUser>.FailAsync(_localizer["User.Phone.AlreadyExists"]);
            }

            return await Result<AppUser>.SuccessAsync();
        }

        private async Task LoadUserBranches(List<AppUser> users)
        {
            List<Guid> branchIds = users.Where(e => e.BranchId.HasValue && e.BranchId != Guid.Empty)
                .Select(e => e.BranchId.Value)
                .Distinct().ToList();

            IEnumerable<Branch> userBranches = await _branchService.GetAllByIds(branchIds).ConfigureAwait(false);
            users.ForEach(e => { e.BranchName = userBranches.FirstOrDefault(b => b.Id == e.BranchId)?.Name; });
        }

        public async Task<Result<AppUser>> GetByIdAsync(Guid id,bool? loadBranch=null)
        {
            var response = await _applictionUserManager.FindByIdAsync(id).ConfigureAwait(false);
            if (!response.Succeeded)
                return response;

            if (loadBranch.HasValue && loadBranch.Value && response.Data.BranchId.IsNotNullOrEmply())
            {
                IEnumerable<Branch> branches = await _branchService
                    .GetAllByIds(new List<Guid> {response.Data.BranchId.Value}).ConfigureAwait(false);

                var enumerable = branches as Branch[] ?? branches.ToArray();
                response.Data.BranchId = enumerable.FirstOrDefault()?.Id;
                response.Data.BranchName = enumerable.FirstOrDefault()?.Name;
                return response;
            }

            return response;
        }

        public async Task<Result> DeleteAsync(AppUser appuser)
        {
            return await _applictionUserManager.DeleteAsync(appuser).ConfigureAwait(false);
        }

        public Task<IEnumerable<DropDownDto<Guid>>> GetUserDropDownAsync(string searchText)
        {
            return _applictionUserManager.GetUserDropDownAsync(searchText);
        }

        public async Task<Result<AppUser>> UpdateStatusAsync(AppUser appUser)
        {
            var result=await _applictionUserManager.UpdateAsync(appUser);
            if (result.Succeeded)
                return await Result<AppUser>.SuccessAsync(data: result.Data, 
                    string.Format(_localizer["User.Status.Changed"],
                                     result.Data.FullName, appUser.IsActive ? "enabled" : "disabled"));
            
            return result;
        }
    }
}