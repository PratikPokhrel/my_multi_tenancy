using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace Core.EF.IdentityModels
{
    // public class ApplicationUser : IdentityUser<Guid>
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public ApplicationUserLogin() : base()
        {
        }
    }
}
