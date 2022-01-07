using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Core.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using my_multi_tenancy.Controllers.Branches.Dtos;
using my_multi_tenancy.Controllers.Branches.Dtos.Resp;
using Services.Services;
using Services.Services.Branches.Resp;
using System.Linq;
using Infrastructure.Filters;
using AutoMapper;
using my_multi_tenancy.Controllers.Branches.Dtos.Rqst;

namespace my_multi_tenancy.Controllers.Branches
{
    /// <summary>
    /// Branch Api
    /// </summary>
    [Route("api/branches")]
    public class BranchController : BaseApiController
    {
        #region Ctor & Properties

        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="branchService"></param>
        /// <param name="mapper"></param>
        public BranchController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
        }

        #endregion


        #region Client APIS

        /// <summary>
        /// Get ALl Branches list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<BranchDto>>>> GetAllAsync([FromQuery] string name = "",[FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var pagedResult = await _branchService.GetAllAsync(name, page, pageSize).ConfigureAwait(false);
            PaginatedResult<BranchDto> returnResult =new(_mapper.Map<List<BranchDto>>(pagedResult.Items), pagedResult.TotalCount, page, pageSize);
            return AppOk(returnResult);
        }

        /// <summary>
        /// Get branch Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<IEnumerable<BranchResp>>>> GetAsync(Guid id)
        {
            var serviceResp = await _branchService.GetAllById(id).ConfigureAwait(false);
            return !serviceResp.Succeeded ? AppOk(serviceResp) : AppOk(serviceResp, _mapper.Map<BranchResp>(serviceResp.Data));
        }

        /// <summary>
        /// Add new branch
        /// </summary>
        /// <param name="branchRqst"></param>
        /// <returns></returns>
        [ModelStateValidator]
        [HttpPost]
        public async Task<ActionResult<Result<BranchDto>>> AddAsync([FromBody] BranchRqst branchRqst)
        {
            var result = await _branchService.AddAsync(branchRqst.MapToEntity()).ConfigureAwait(false);
            if (!result.Succeeded)
                return AppOk(result, branchRqst);

            branchRqst.Id = result.Data.Id;
            return AppOk(result, branchRqst);
        }

        /// <summary>
        /// Update Branch
        /// </summary>
        /// <param name="branchRqst"></param>
        /// <returns></returns>
        [HttpPut]
        [ModelStateValidator]
        public async Task<ActionResult<Result<BranchDto>>> UpdateAsync([FromBody] BranchRqst branchRqst)
        {
            Result<BranchServiceResp> result = await _branchService.GetAllById(branchRqst.Id).ConfigureAwait(false);
            if(!result.Succeeded)
                return AppOk(result,branchRqst);

            return !result.Succeeded ? AppOk(result) : AppOk(result, branchRqst);
        }

        /// <summary>
        /// Delete Branch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<Result<BranchDto>>> DeleteAsync(Guid id)
        {
            var result = await _branchService.GetAllById(id).ConfigureAwait(false);
            if (!result.Succeeded)
                return AppOk(result);

            var resultResult = await _branchService.DeleteAsync(id).ConfigureAwait(false);
            return AppOk(resultResult);
        }

        /// <summary>
        /// Get Branches drop down
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet, Route("drop-down")]
        public async Task<ActionResult<Result<IEnumerable<DropDownDto<Guid>>>>> GetDropDownAsync([FromQuery] string name)
        {
            return AppOk(await _branchService.GetDropDownAsync(name).ConfigureAwait(false));
        }

        #endregion
    }
}