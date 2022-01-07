using Core.Dto;
using Core.Dto.Security;
using Core.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Identity
{
    public interface IUserService
    {
        /// <summary>
        /// Get all users paginated
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="roleId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PaginatedResult<AppUser>> GetAllUsers(string searchText, Guid? roleId, int page, int pageSize);
        
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<AppUser>> GetByIdAsync(Guid id,bool? loadBranch = null);
        
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        Task<Result> DeleteAsync(AppUser appUser);
        
        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        Task<Result<AppUser>> UpdateAsync(AppUser appUser);


         /// <summary>
        /// Update user
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        Task<Result<AppUser>> UpdateStatusAsync(AppUser appUser);
        
        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<AppUser>> AddAsync(AppUser user);
        
        /// <summary>
        /// Get users drop down
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        Task<IEnumerable<DropDownDto<Guid>>> GetUserDropDownAsync(string searchText);
    }
}
