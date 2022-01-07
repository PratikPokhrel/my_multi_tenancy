using Core.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.DataAccess
{
    public interface IPager
    {
        Task<PaginatedResult<Entity>> ConvertToPagedListAsync<Entity>(IQueryable<Entity> source, int pageIndex, int pageSize) where Entity : class;
    }
}
