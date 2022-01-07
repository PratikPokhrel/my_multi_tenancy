using Core.EF.Data.Configuration.Pg;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EF.Seed
{
    public static class Seeder
    {
        public static async Task CreateSeederAsync(this DeviceContext deviceContext)
        {
           await CreateCategoriesAsync(deviceContext).ConfigureAwait(false);
           await CreateMenusAsync(deviceContext).ConfigureAwait(false);
           await deviceContext.SaveChangesAsync();
        }

        public static async Task CreateCategoriesAsync(DeviceContext deviceContext)
        {
            if (!(await deviceContext.Category.IgnoreQueryFilters().AnyAsync().ConfigureAwait(false)))
            {
                var categories = new List<Category>
                {
                    new Category{Name="Hr",CreatedOn=DateTime.Now.ToUniversalTime()},
                    new Category{Name="Accounting",CreatedOn=DateTime.Now.ToUniversalTime()},
                    new Category{Name="Finance",CreatedOn=DateTime.Now.ToUniversalTime()},
                    new Category{Name="It & Communication",CreatedOn=DateTime.Now.ToUniversalTime()},
                    new Category{Name="All",CreatedOn=DateTime.Now.ToUniversalTime()},
                };
                await deviceContext.Category.AddRangeAsync(categories);
            }
        }

        public static async Task CreateMenusAsync(DeviceContext deviceContext)
        {
            if (!(await deviceContext.Menu.IgnoreQueryFilters().AnyAsync().ConfigureAwait(false)))
            {
                var categories = new List<Menu>
                {
                    new Menu{Id=1,Name="Employee",Order=1,IsPaarent=true,ParentId=0},
                    new Menu{Id=2,Name="Employee List",Order=11,IsPaarent=false,ParentId=1},
                    new Menu{Id=3,Name="Employee Detail",Order=12,IsPaarent=false,ParentId=1},
                    new Menu{Id=4,Name="Parent",Order=2,IsPaarent=true,ParentId=0},
                    new Menu{Id=5,Name="Parent List",Order=21,IsPaarent=false,ParentId=4},
                    new Menu{Id=6,Name="Employee Detail",Order=22,IsPaarent=false,ParentId=4},

                };
                await deviceContext.Menu.AddRangeAsync(categories);
            }
        }
    }
}
