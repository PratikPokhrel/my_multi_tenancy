using Core.Entities.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Department : Entity<int>
    {

        public Department()
        {
            Category = new HashSet<Category>();
            SubDepartment = new HashSet<SubDepartment>();
        }
        public string Name { get; set; }

        public virtual ICollection<Category> Category { get; set; }
        public virtual ICollection<SubDepartment> SubDepartment { get; set; }
    }

    public class SubDepartment : Entity<int>
    {
        public string Name { get; set; }
        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
