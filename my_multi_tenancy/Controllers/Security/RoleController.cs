using Core.Dto;
using Core.Dto.Security;
using Core.Infrastructure.Pagination;
using Core.Security.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Dtos.Resp.Identity;
using Services.Services.Dtos.Rqst.Identity;
using Services.Services.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace my_multi_tenancy.Controllers.Security
{
    [Route("api/roles")]
    public class RoleController : BaseApiController
    {
        #region "Ctor & Properties"
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        #endregion


        #region "Client API's"
        /// <summary>
        /// Get all roles
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loadClaims"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<AppRole>>>> GetAllAsync(string name, bool loadClaims = false)
        {
            return AppOk(await _roleService.GetAllAsync(name, loadClaims).ConfigureAwait(false));
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<RoleResp>>> GetAsync(Guid id)
        {
            var result=await _roleService.GetAsync(id).ConfigureAwait(false);
            return AppOk(result);
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<Result<RoleResp>>> DeleteAsync(Guid id)
        {
            return AppOk(await _roleService.DeleteAsync(id).ConfigureAwait(false));
        }

        /// <summary>
        /// Add new role
        /// </summary>
        /// <param name="rqst"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Result<RoleResp>>> AddAsync([FromBody] RoleRqst rqst)
        {
            var resp = await _roleService.CreateAsync(rqst).ConfigureAwait(false);
            if(!resp.Succeeded)
                return AppOk(resp);

            rqst.Id=resp.Data.Id;
            return AppOk(resp,rqst);
        }


        /// <summary>
        /// Update role
        /// </summary>
        /// <param name="rqst"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<Result<RoleResp>>> UpdateAsync([FromBody] RoleRqst rqst)
        {
            var resp = await _roleService.UpdateAsync(rqst).ConfigureAwait(false);
            if (!resp.Succeeded)
                return AppOk(resp);

            return AppOk(resp, rqst);
        }


        [HttpPost, Route("claims")]
        public async Task<ActionResult<RoleResp>> AddClaimAsync([FromBody] AppRoleClaim appRoleClaim)
        {
            return AppOk(await _roleService.AddClaimAsync(appRoleClaim).ConfigureAwait(false));
        }
        #endregion

    }
}
