using Core.Entities.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Branch : FullAudited<Guid>
    {
        public Branch()
        {

        }
        public string Name { get; set; }
        public bool IsDefault { get; set; }


        public  virtual ICollection<BranchUser> BranchUser { get; set; }
    }
}
