using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace Core.EF.IdentityModels
{
    // public class ApplicationUser : IdentityUser<Guid>
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName)
        {
            Name = roleName;
        }
    }
}
