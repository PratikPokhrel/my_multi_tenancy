using Core.Entities.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BranchUser:FullAudited<int>
    {
        public Guid BranchId { get; set; }
        public Guid UserId { get; set; }

        public virtual Branch Branch { get; set; }
    }
}
