using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace Core.EF.IdentityModels
{
    // public class ApplicationUser : IdentityUser<Guid>
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public ApplicationRoleClaim() : base()
        {
        }
        public virtual ApplicationRole  ApplicationRole { get; set; }
    }
}
