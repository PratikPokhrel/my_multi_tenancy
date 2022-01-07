using Core.Dto.Security;
using System.Collections.Generic;

namespace my_multi_tenancy.Controllers.Branches.Dtos.Rqst
{
    public class BranchResp:BranchDto
    {
        public BranchResp()
        {
            Users = new List<AppUser>();
        }
        public List<AppUser> Users { get; set; }
    }
}
