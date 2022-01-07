using Core.Dto.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Branches.Resp
{
    public class BranchServiceResp
    {
        public BranchServiceResp()
        {
            Users = new List<AppUser>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public IEnumerable<AppUser> Users { get; set; }
    }
}
