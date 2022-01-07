using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Core.Entities;
using Core.Infrastructure.Pagination;
using Services.Services.Branches.Resp;

namespace Services.Services
{
    public interface IBranchService
    {
        /// <summary>
        /// Get all branch list paginated
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<PaginatedResult<Branch>> GetAllAsync(string name, int page, int pageSize);
        /// <summary>
        /// Get branch by Ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<Branch>> GetAllByIds(List<Guid> ids);


        /// <summary>
        /// Get Branch by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<BranchServiceResp>> GetAllById(Guid id);
        /// <summary>
        /// Add New branch
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        Task<Result<Branch>> AddAsync(Branch branch);

        /// <summary>
        /// Update branch
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        Task<Result<Branch>> UpdateAsync(Branch branch);


        /// <summary>
        /// Delete branch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<Branch>> DeleteAsync(Guid id);

        /// <summary>
        /// Get Branch drop down
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DropDownDto<Guid>>> GetDropDownAsync(string search);
    }
}
