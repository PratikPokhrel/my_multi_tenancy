using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Dtos.Resp.Identity
{
    public class RoleResp
    {
        public RoleResp()
        {
            Claims = new List<RoleClaimResp>();
        }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<RoleClaimResp> Claims { get; set; }
    }

    public class RoleClaimResp
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
