using Core;
using Core.Constants;
using Core.Dto;
using Core.Dto.Security;
using Core.Infrastructure;
using Core.Security.User;
using LazyCache;
using Microsoft.Extensions.Localization;
using Services.Services.Dtos.Resp.Identity;
using Services.Services.Dtos.Rqst.Identity;
using Services.Services.Identity.Mapper;

namespace Services.Services.Identity
{
    public class RoleService : IRoleService
    {
        #region "Ctor & Properties"
        private readonly IRoleManager _roleManager;
        private readonly IStringLocalizer<RoleService> _localizer;
        private readonly IRoleClaimManager _roleClaimManager;
        private readonly IAppCache _lazyCache;
        private readonly IApplictionUserManager _applictionUserManager;
        private readonly IStringLocalizer _commonLocalizer;
        private static readonly string serviceName="Role";
        public RoleService(IRoleManager roleManager,IRoleClaimManager roleClaimManager,IApplictionUserManager applictionUserManager, IStringLocalizer<RoleService> localizer, ICommonLocalizer commonLocalizer, IAppCache cache)
        {
            _roleManager = roleManager;
            _localizer = localizer;
            _roleClaimManager = roleClaimManager;
            _applictionUserManager = applictionUserManager;
            _lazyCache = cache;
            _commonLocalizer = commonLocalizer.Localize;
        }

        #endregion


        #region "Methods"
        public async Task<IEnumerable<RoleResp>> GetAllAsync(string name,bool loadClaims)
        {
            Func<Task<IEnumerable<AppRole>>> getAllRoles = async() 
                => await _roleManager.GetAllAsync(name, loadClaims).ConfigureAwait(false);
            var roles =await getAllRoles();
            return roles.ToResp();
        }

        public async Task<Result<RoleResp>> CreateAsync(RoleRqst role)
        {
            var validate = await ValidateAddEdit(role.Name).ConfigureAwait(false);
            if (!validate.Succeeded)
                return validate;

            Result result = await _roleManager.CreateAsync(role.ToAppRole()).ConfigureAwait(false);
            if (!result.Succeeded)
                return await Result<RoleResp>.FailAsync(messages: result.Messages,MessageTypes.Warning);

            _lazyCache.Remove(CacheKeys.GET_ALL_ROLES);
            return await Result<RoleResp>.SuccessAsync(data: role.ToResp(),_commonLocalizer["Added"].FormatWith(serviceName, role.Name));
          
        }

        public async Task<Result<RoleResp>> UpdateAsync(RoleRqst role)
        { 
            //Get role by id,if not exists, return;
         
            //if another role with same name exists, return;
            var validate = await ValidateAddEdit(role.Name,role.Id).ConfigureAwait(false);
            if (!validate.Succeeded)
                return validate;

            Result result = await _roleManager.UpdateAsync(role.ToAppRole()).ConfigureAwait(false);
            if (!result.Succeeded)
                return await Result<RoleResp>.FailAsync(result.Messages);

            _lazyCache.Remove(CacheKeys.GET_ALL_ROLES);
            return await Result<RoleResp>.SuccessAsync(role.ToResp(),_commonLocalizer["Updated"].FormatWith(serviceName, role.Name));
            
        }

        private async Task<Result<RoleResp>> ValidateAddEdit(string roleName,Guid? roleId=null)
        {
           var isExists= await _roleManager.ExistsAsync(roleName,roleId).ConfigureAwait(false);
           return isExists ? await Result<RoleResp>.FailAsync(_commonLocalizer["AlreadyExists"].FormatWith(roleName)) 
                           : await Result<RoleResp>.SuccessAsync(roleName);
        }

        public async Task<Result<RoleResp>> DeleteAsync(Guid id)
        {
            var dbRole = await GetAsync(id).ConfigureAwait(false);
            if (!dbRole.Succeeded)
                return await Result<RoleResp>.FailAsync(_commonLocalizer["NotFound"]);

            await _roleManager.DeleteAsync(id).ConfigureAwait(false);
            return await Result<RoleResp>.SuccessAsync(_localizer["Deleted"].FormatWith(serviceName));
        }

        public async Task<Result> AddClaimAsync(AppRoleClaim roleClaim)
        {
            await _roleClaimManager.AddAsync(roleClaim);
            return (Result)await Result.SuccessAsync();
        }

        public async Task<Result<RoleResp>> GetAsync(Guid id)
        {
            var role=await _roleManager.FindByIdAsync(id).ConfigureAwait(false);
            return role == null
                ? await Result<RoleResp>.FailAsync(_commonLocalizer["NotFound"])
                : await Result<RoleResp>.SuccessAsync(data: role.ToResp());
        }
        #endregion

    }
}
