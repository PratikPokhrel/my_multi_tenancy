using Core.Dto;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Branches;
using System;
using System.Threading.Tasks;

namespace my_multi_tenancy.Controllers.Branches
{
    [Route("api/branch-users")]
    public class BranchUserController : BaseApiController
    {
        private IBranchUserService _branchUserService;

        public BranchUserController(IBranchUserService branchUserService)
        {
            _branchUserService = branchUserService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> AddAsync([FromBody] BranchUserDto dto)
        {
            var added=await _branchUserService.AddAsync(dto.MapToEntity()).ConfigureAwait(false);
            if(!added.Succeeded)
                return AppOk(added);

            return AppOk(added);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetBranchUsersAsync(Guid id)
        {
            var added = await _branchUserService.GetBranchUsers(id).ConfigureAwait(false);
            return AppOk(added);
        }

        [HttpDelete]
        public async Task<ActionResult<Result>> DeleteAsync(int id)
        {
            return AppOk(await _branchUserService.DeleteAsync(id).ConfigureAwait(false));
        }
    }

    public class BranchUserDto
    {
        public Guid BranchId{ get; set; }
        public Guid UserId{ get; set; }
    }

    public static class Mapper
    {
        public static BranchUser MapToEntity(this BranchUserDto dto)
        {
            if(dto == null) return null;
            return new BranchUser
            {
                BranchId = dto.BranchId,
                UserId = dto.UserId,
            };
        }
    }
}
