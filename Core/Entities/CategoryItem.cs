using Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CategoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }

        [NavigationProperty]
        public virtual Category Category  { get; set; }

        public virtual ICollection<CategoryItemChildren> CategoryItemChildrens { get; set; }
    }
}
