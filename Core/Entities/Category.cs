using Core.Entities.Audit;
using Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Category:FullAudited<int>
    {
        public Category()
        {
            CategoryItem = new HashSet<CategoryItem>();
        }
        public string Name { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<CategoryItem> CategoryItem { get; set; }
    }
}
