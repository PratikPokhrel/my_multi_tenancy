using System;
using System.Collections.Generic;
using Core.Entities.Audit;
using Microsoft.AspNetCore.Identity;


namespace Core.EF.IdentityModels
{
    public class ApplicationRole : IdentityRole<Guid>,IFullAudited<Guid>, IHasTenant
    {
        public ApplicationRole() : base()
        {
            ApplicationUserRoles = new List<ApplicationUserRole>();
        }
       
        public ApplicationRole(Guid id,string roleName)
        {
            Id = id;
            Name = roleName;
        }

        public ApplicationRole(string roleName)
        {
            Name = roleName;
        }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public virtual bool IsDeleted { get; set; }
        public Guid TenantId { get; set; }
        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
    }
}
