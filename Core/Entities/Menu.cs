using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Audit;

namespace Core.Entities
{
    public class Menu:FullAudited<int>
    {
        public string Name{get;set;}
        public int Order { get; set; }
        public bool IsPaarent { get; set; }
        public int ParentId { get; set; }
        public bool IsOldLink { get; set; }
        public string Icon { get; set; }  

        [NotMapped]
        public List<Menu> SubMenus { get; set; }
    }


}