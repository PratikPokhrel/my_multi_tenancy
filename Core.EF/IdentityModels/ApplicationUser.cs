using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Core.Entities.Audit;

namespace Core.EF.IdentityModels
{
    public class ApplicationUser : IdentityUser<Guid>,IFullAudited<Guid>,IHasTenant
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
        public bool IsActive { get; set; }

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
