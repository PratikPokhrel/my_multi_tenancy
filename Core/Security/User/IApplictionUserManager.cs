using Core.Dto;
using Core.Dto.Security;
using Core.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Security.User
{
    public interface IApplictionUserManager
    {
        Task<bool> IsUserInTenantAsync(Guid userId,Guid tenantId);
        Task<PaginatedResult<AppUser>> GetAllUsers(string searchText,Guid? roleId, int page, int pageSize);
        Task<Result<AppUser>> FindByIdAsync(Guid id);

        /// <summary>
        /// Get users containing given Ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<AppUser>> GetManyAsync(List<Guid> ids);
        Task<Result> DeleteAsync(AppUser appUser);
        Task<ApplicationIdentityResult> AddAsync(AppUser user);
        Task<Result<AppUser>> UpdateAsync(AppUser user);
        Task<IEnumerable<DropDownDto<Guid>>> GetUserDropDownAsync(string searchText);
        Task<Result> DoesUsernameAlreadyExistsAsync(string userName);
        Task<Result> DoesPhoneAlreadyExistsAsync(string userName);


        /// <summary>
        /// Sign in by username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> PasswordSignInAsync(string userName, string password);

        Task<AppUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task<IList<Claim>> GetClaimsAsync(AppUser user);
    }
}
