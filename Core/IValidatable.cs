using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IValidatable
    {
        /// <summary>
        /// The classes should provide validation logic
        /// </summary>
        void Validate();
    }
}
