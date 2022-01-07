using Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Settings
{
    /// <summary>
    /// Class to read application settings from the appsettings file
    /// </summary>
    public class AppSettings : IValidatable
    {
        /// <summary>
        /// Default thread culture
        /// </summary>
        public string DefaultCulture { get; set; }

        /// <summary>
        /// All supported cultures
        /// </summary>
        public List<string> SupportedCultures { get; set; }

        /// <summary>
        /// All supported UI cultures
        /// </summary>
        public List<string> SupportedUiCultures { get; set; }

        /// <inherit/>
        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);

            bool valid = true;
            

            if (!valid)
                throw new ValidationException("Invalid culture code in the config file");
        }
    }
}
