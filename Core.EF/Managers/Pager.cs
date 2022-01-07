using Core.Infrastructure.DataAccess;
using Core.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.EF.Data;

namespace Core.EF.Managers
{
    public class Pager : IPager
    {
        public async Task<PaginatedResult<Entity>> ConvertToPagedListAsync<Entity>(IQueryable<Entity> source, int pageIndex, int pageSize) where Entity : class
        {
            return await source.ToPaginatedListAsync(pageIndex, pageSize).ConfigureAwait(false);
        }
    }
}
