using Core;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Settings
{
    /// <summary>
    /// Authentication configuration settings like identityserver address
    /// </summary>
    public class AuthenticationSettings : IValidatable
    {
        /// <summary>
        /// IdenityServer4 authorization end point
        /// </summary>
        [Required]
        public string AuthorizationUrl { get; set; }

        /// <summary>
        /// Api scope
        /// </summary>
        [Required]
        public string Scope { get; set; }


        /// <summary>
        /// JWT secret key
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Validate if entered values are valid
        /// </summary>
        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}
