using Core.Entities;
using Core.Infrastructure.DataAccess;
using MediatR;
using Services.Services;
using Services.Services.Dtos.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Features.Menus.Queries
{
    public class GetAllMenuQuery : IRequest<IEnumerable<MenuResp>>
    {
    }

    internal class GetAllMenuQueryHandler : IRequestHandler<GetAllMenuQuery, IEnumerable<MenuResp>>
    {
        private readonly IRepository<Menu> _menuRepo;
        private readonly IPager _pager;

        public GetAllMenuQueryHandler(IUnitOfWork unitOfWork, IPager pager)
        {
            _menuRepo=unitOfWork.GetRepository<Menu>();
            _pager = pager;
        }

        public async Task<IEnumerable<MenuResp>> Handle(GetAllMenuQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Menu, MenuResp>> select = e => new MenuResp
            {
                Id = e.Id,
                IsPaarent = e.IsPaarent,
                Name = e.Name,
                Order = e.Order,
                ParentId = e.ParentId,
                Icon = e.Icon,
            };
            var paged = await _pager.ConvertToPagedListAsync(_menuRepo.TableNoTraking.Select(select), 1, 10);

            IEnumerable<Menu> menus = await _menuRepo
                                                      .GetAllAsync(where: e => true, asNoTracking: true)
                                                      .ConfigureAwait(false);
            IEnumerable<Menu> hierarchy = menus
                                              .Where(c => c.ParentId == 0)
                                              .Select(c => new Menu()
                                              {
                                                  Id = c.Id,
                                                  Name = c.Name,
                                                  ParentId = c.ParentId,
                                                  SubMenus = GetChildren(menus, c.Id)
                                              });

            return HierarchyWalk(hierarchy).ToResp();
        }

        private static List<Menu> GetChildren(IEnumerable<Menu> menus, int parentId)
        {
            return menus
                    .Where(c => c.ParentId == parentId)
                    .Select(c => new Menu
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ParentId = c.ParentId,
                        SubMenus = GetChildren(menus, c.Id)
                    }).ToList();
        }

        private static List<Menu> HierarchyWalk(IEnumerable<Menu> hierarchy)
        {
            List<Menu> children = new ();
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    children.Add(item);
                    HierarchyWalk(item.SubMenus);
                }
                return children;
            }
            return children;
        }

       
    }
}
