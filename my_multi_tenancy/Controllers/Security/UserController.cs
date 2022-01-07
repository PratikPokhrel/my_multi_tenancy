using Core.Dto;
using Core.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Dtos.Rqst.Identity;
using Services.Services.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using my_multi_tenancy.Controllers.Security.Mappers;
using my_multi_tenancy.Controllers.Security.Dtos.Resp;
using System.Linq;
using Core.Dto.Security;
using my_multi_tenancy.Controllers.Base;
using Core.Infrastructure;
using AutoMapper;

namespace my_multi_tenancy.Controllers.Security
{
    /// <summary>
    /// User Controller
    /// </summary>
    [Route("v1/users")]
    public class UserController : BasePublicController
    {
        #region "Ctor & Properties"
        private readonly IUserService _userService;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,IExcelService excelService,IMapper mapper)
        {
            _userService = userService;
            _excelService = excelService;
            _mapper = mapper;
        }
        #endregion


        #region "API's"
        /// <summary>
        /// Get user list paginated
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="roleId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<PaginatedResult<AppUser>>>> GetAllAsync([FromQuery] string searchText,
                                                       [FromQuery] Guid? roleId, [FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            var pagedResult = await _userService.GetAllUsers(searchText, roleId, page, pageSize).ConfigureAwait(false);
            var mappedResult = new PaginatedResult<UserListResp>(pagedResult.Items.ToList().Select(e => _mapper.Map<UserListResp>(e)).ToList())
            {
                CurrentPage = pagedResult.CurrentPage,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages
            };

            return AppOk(mappedResult);
        }


        /// <summary>
        /// Get user drop down list
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet,Route("drop-down")]
        public async Task<ActionResult<Result<IEnumerable<DropDownDto<Guid>>>>> GetDropDownAsync([FromQuery] string searchText)
          =>
           AppOk(await _userService.GetUserDropDownAsync(searchText).ConfigureAwait(false));

        /// <summary>
        /// Get single user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<AppUser>>> GetAsync(Guid id)
        {
            var resp = await _userService.GetByIdAsync(id,true).ConfigureAwait(false);
            return !resp.Succeeded ? AppOk(resp) : AppOk(resp, _mapper.Map<UserListResp>(resp.Data));
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> DeleteAsync(Guid id)
        {
           var serviceResp=await _userService.GetByIdAsync(id).ConfigureAwait(false);
            return !serviceResp.Succeeded ? AppOk(serviceResp) : AppOk(await _userService.DeleteAsync(serviceResp.Data).ConfigureAwait(false));
        }


        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="rqst"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Result<AppUser>>> AddAsync([FromBody] UserRqst rqst)
        {
            var result = await _userService.AddAsync(rqst.ToAppUser()).ConfigureAwait(false);
            if(!result.Succeeded)
                return AppOk(result);

            rqst.Id=result.Data.Id;
            return AppOk(result,rqst);
        }


        /// <summary>
        /// Update existing user
        /// </summary>
        /// <param name="rqst"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<Result<AppUser>>> UpdateAsync([FromBody]UserRqst rqst)
        {
            return  AppOk(await _userService.UpdateAsync(rqst.ToAppUser()).ConfigureAwait(false));
        }

        

        /// <summary>
        /// Update user status(active/inactive)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut,Route("update-status")]
        public async Task<ActionResult<Result<AppUser>>> UpdateStatusAsync(Guid userId,bool status)
        {
            var serviceResult=await _userService.GetByIdAsync(userId).ConfigureAwait(false);
            if(!serviceResult.Succeeded)
                return AppOk(serviceResult);

            serviceResult.Data.IsActive=status;
            return  AppOk(await _userService.UpdateStatusAsync(serviceResult.Data).ConfigureAwait(false));
        }

        #endregion
    }
}
