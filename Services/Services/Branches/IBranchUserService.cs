using Core.Dto;
using Core.Dto.Security;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Branches
{
    public interface IBranchUserService
    {
        /// <summary>
        /// Get all users associated with branch
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        Task<IEnumerable<AppUser>> GetBranchUsers(Guid branchId);

        /// <summary>
        /// Add user to branch
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<BranchUser>> AddAsync(BranchUser user);

        /// <summary>
        /// Delete branch user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<BranchUser>> DeleteAsync(int id);
    }
}
