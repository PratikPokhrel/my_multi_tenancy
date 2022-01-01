using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Core.Entities.Audit;
using Microsoft.AspNetCore.Identity;


namespace Core.EF.IdentityModels
{
    // public class ApplicationUser : IdentityUser<Guid>
    public class ApplicationRole : IdentityRole<Guid>,IFullAudited<Guid>
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

        [JsonIgnore]
        public Guid CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedOn { get; set; }
        [JsonIgnore]
        public Guid? ModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? ModifiedOn { get; set; }
        [JsonIgnore]
        public Guid? DeletedBy { get; set; }
        [JsonIgnore]
        public DateTime? DeletedOn { get; set; }
        [JsonIgnore]
        public virtual bool IsDeleted { get; set; }
        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
    }
}
