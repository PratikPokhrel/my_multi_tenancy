using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Identity.Rqst
{
    public class TokenRequest
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public TokenRequest()
        {

        }
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; } = "pratik@123";


        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; } = "Admin@123";
    }
}
