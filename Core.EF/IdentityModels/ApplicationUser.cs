using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Core.EF.IdentityModels
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    //If you make changes to the User Table -- 
    //You need to update this table as well as the User table in SurgeOne.DAL.Entities
    //As they have to be in sync -- as ApplicationUser gets loaded by the ApplicationContext. and User gets loaded by the DBOContext
    //We only put properties in here that drive the application - everything else should go in UserSetting
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string ImageUrl { get; set; }

        [NotMapped]
        public string PassWord { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }
    }
}
