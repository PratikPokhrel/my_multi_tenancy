using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Security
{
    public class ApplicationIdentityResult
    {
        public List<string> Errors
        {
            get;
            private set;
        }

        public bool Succeeded
        {
            get;
            private set;
        }

        public ApplicationIdentityResult(List<string> errors, bool succeeded)
        {
            Succeeded = succeeded;
            Errors = errors;
        }
    }
}
