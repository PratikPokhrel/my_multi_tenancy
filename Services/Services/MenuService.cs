using Core.Dto;
using Core.Entities;
using Core.Infrastructure;
using Core.Infrastructure.DataAccess;
using Microsoft.Extensions.Localization;
using Services.Services.Dtos.Resp;
using Services.Services.Dtos.Rqst;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menuRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<MenuService> _localizer;

        public MenuService(IUnitOfWork unitOfWork, IStringLocalizer<MenuService> localizer)
        {
            _menuRepo=unitOfWork.GetRepository<Menu>();
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<Menu>> AddAsync(Menu menu)
        {
            var validate = await ValidateCommandAsync(menu).ConfigureAwait(false);
            if (!validate.Succeeded)
                return validate;

            var addedMenu= await _menuRepo.AddAsync(menu).ConfigureAwait(false);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);
            return await Result<Menu>.SuccessAsync(addedMenu, string.Format(_localizer["Menu {0} Created successfully."], menu.Name));
        }


        private async Task<Result<Menu>> ValidateCommandAsync(Menu menu)
        {
            if(await _menuRepo.AnyAsync(e=>e.Name.ToLower().Equals(menu.Name.ToLower())).ConfigureAwait(false))
                return await Result<Menu>.SuccessAsync("Already contains menu with same name");

            if (await _menuRepo.AnyAsync(e =>e.Order==menu.Order).ConfigureAwait(false))
                return await Result<Menu>.SuccessAsync("Already contains menu with same order");

            return await Result<Menu>.SuccessAsync();
        }

        public async Task<IEnumerable<MenuResp>> GetMenusAsync()
        {
            //var temm=await _unitOfWork.GetRepository<Category>().GetAsyncTempList(e=>true, "Department",
            //                                                                               "Department.SubDepartment",
            //                                                                               "CategoryItem", 
            //                                                                               "CategoryItem.CategoryItemChildrens");
            //var _catRepo = _unitOfWork.GetRepository<Category>();
            //var adasd= _unitOfWork.GetRepository<Category>()
            //                           .GetAll(1,20,e=>e.CategoryItem).ToList();

            var aabc = _menuRepo.TableNoTraking
                .Where(e => !_menuRepo.TableNoTraking.Any()).ToList();
            IEnumerable<Menu> menus = await _menuRepo.GetAllAsync(where:e=>true,asNoTracking:true)
                                                     .ConfigureAwait(false);
            IEnumerable<Menu> hierarchy = menus
                                              .Where(c => c.ParentId == 0)
                                              .Select(c => new Menu()
                                              {
                                                  Id = c.Id,
                                                  Name = c.Name,
                                                  ParentId = c.ParentId,
                                                  Path=c.Path,
                                                  SubMenus = GetChildren(menus, c.Id)
                                              });

            return HieararchyWalk(hierarchy).ToResp();
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

        private static List<Menu> HieararchyWalk(IEnumerable<Menu> hierarchy)
        {
            var aItems = new List<Menu>();
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    aItems.Add(item);
                    HieararchyWalk(item.SubMenus);
                }
                return aItems;
            }
            return aItems;
        }
    }

    public class MultipleReader
    {
        public IEnumerable<Menu> Menus { get; set; }
        public IEnumerable<Branch> Branches { get; set; }
    }
}
