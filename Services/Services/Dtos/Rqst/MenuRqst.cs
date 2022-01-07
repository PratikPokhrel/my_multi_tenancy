using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Dtos.Rqst
{
    public class MenuRqst
    {
        public MenuRqst()
        {
            SubMenus = new List<MenuRqst>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsPaarent { get; set; }
        public int ParentId { get; set; }
        public bool IsOldLink { get; set; }
        public string Icon { get; set; }
        public List<MenuRqst> SubMenus { get; set; }
    }
}
