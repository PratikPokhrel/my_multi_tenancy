using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.QueryModels
{
    public class CategoryItemQuery
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryItemName { get; set; }
    }
}
