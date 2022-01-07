using Core.Dto.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Dtos.Rqst.Identity
{
    public class UserRqst
    {
        public UserRqst()
        {
            Roles = new List<KeyValue>();
        }
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime? DOB { get; set; }
        public List<KeyValue> Roles { get; set; }
    }
}
