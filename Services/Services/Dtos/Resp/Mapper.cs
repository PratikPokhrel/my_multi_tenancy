using Core.Entities;
using Services.Services.Dtos.Rqst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Dtos.Resp
{
    public static class Mapper
    {
        public static MenuResp ToResp(this Menu e)
        {
            return new MenuResp
            {
                Id = e.Id,
                IsPaarent = e.IsPaarent,
                Name = e.Name,
                Order = e.Order,
                ParentId = e.ParentId,
                Icon = e.Icon,
                SubMenus = ToResp(e.SubMenus).ToList()
            };
        }

        public static IEnumerable<MenuResp> ToResp(this IEnumerable<Menu> menus)
        {
            if (menus == null) return Enumerable.Empty<MenuResp>();
            return menus.Select(m => ToResp(m));
        }

        public static Menu ToEntity(this MenuRqst e)
        {
            return new Menu { Id = e.Id, IsPaarent = e.IsPaarent, Name = e.Name, Order = e.Order,Icon=e.Icon, ParentId = e.ParentId};
        }
    }
}
