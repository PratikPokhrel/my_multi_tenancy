using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Security
{
    public class AppRole
    {
        public AppRole()
        {
            RoleClaims = new List<AppRoleClaim>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<AppRoleClaim> RoleClaims { get; set; }
    }
}
