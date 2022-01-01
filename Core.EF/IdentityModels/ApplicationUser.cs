using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Core.Entities.Audit;
using System.Text.Json.Serialization;

namespace Core.EF.IdentityModels
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    //If you make changes to the User Table -- 
    //You need to update this table as well as the User table in SurgeOne.DAL.Entities
    //As they have to be in sync -- as ApplicationUser gets loaded by the ApplicationContext. and User gets loaded by the DBOContext
    //We only put properties in here that drive the application - everything else should go in UserSetting
    public class ApplicationUser : IdentityUser<Guid>,IFullAudited<Guid>
    {
        public ApplicationUser()
        {
            ApplicationUserRoles = new HashSet<ApplicationUserRole>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string ImageUrl { get; set; }
        public Guid TenantId { get; set; }
        public Guid BranchId { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public virtual bool IsDeleted { get; set; }

        [NotMapped]
        public string PassWord { get; set; }
        [NotMapped]
        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
    }
}
