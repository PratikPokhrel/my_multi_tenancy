using System;
using System.ComponentModel.DataAnnotations;

namespace my_multi_tenancy.Controllers.Security.Dtos.Resp
{
    public class UserListResp
    {
        public UserListResp()
        {
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime? DOB { get; set; }
        public Guid? RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid? BranchId { get; set; }
        public string BranchName { get; set; }
    }
}
