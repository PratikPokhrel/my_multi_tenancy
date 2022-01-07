using Core.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure
{
    public interface IExcelService
    {
        Task<string> ExportAsync<TData>(IEnumerable<TData> data
            , Dictionary<string, Func<TData, object>> mappers
            , string sheetName = "Sheet1");

        Task<Result<IEnumerable<TEntity>>> ImportAsync<TEntity>(Stream data
            , Dictionary<string, Func<DataRow, TEntity, object>> mappers
            , string sheetName = "Sheet1");
    }
}
