using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Security
{
    public class AppUser
    {
        public AppUser()
        {
            Roles = new List<KeyValue>();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime? DOB { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string BranchName { get; set; }
        public Guid? BranchId { get; set; }
        public bool IsActive { get; set; }
        public List<KeyValue> Roles { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public string Creator { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }


    public class KeyValue
    {
        public KeyValue(Guid id,string name)
        {
            Id=id;
            Name=name;  
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
