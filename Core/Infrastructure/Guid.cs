using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure
{
    public static class GuidExtensions
    {
        public static bool IsNotNullOrEmply(this Guid? guid)
        {
            return  guid.HasValue && guid != Guid.Empty ;
        }
    }
   
}
