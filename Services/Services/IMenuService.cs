using Core.Dto;
using Core.Entities;
using Services.Services.Dtos.Rqst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuResp>> GetMenusAsync();
        Task<Result<Menu>> AddAsync(Menu menu);
    }

    public class MenuResp
    {
        public MenuResp()
        {
            SubMenus=new List<MenuResp>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsPaarent { get; set; }
        public int ParentId { get; set; }
        public bool IsOldLink { get; set; }
        public string Icon { get; set; }
        public List<MenuResp> SubMenus { get; set; }
    }
}
