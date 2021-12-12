using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace Core.EF.IdentityModels
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public ApplicationUserRole() : base()
        {
        }
    }
}
