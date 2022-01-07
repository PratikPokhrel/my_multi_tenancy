using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Audit
{
    public interface IHasTenant
    {
        public Guid TenantId { get; set; }
    }
}
