using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class NavigationPropertyAttribute : Attribute
    {
        public NavigationPropertyAttribute()
        {
        }
    }
}
