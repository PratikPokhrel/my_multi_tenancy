using Core.Entities.Audit;
using Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CategoryItemChildren:Entity<int>
    {
        public string Description { get; set; }

        [NavigationProperty]
        public virtual CategoryItem  CategoryItem{ get; set; }
    }
}
