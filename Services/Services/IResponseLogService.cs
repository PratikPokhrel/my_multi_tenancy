using System.Threading.Tasks;
using Core.Entities;

namespace Services.Services
{
    public interface IResponseLogService
    {
       Task AddAsync(object logs,string actionName,string apiName);
       Task<IEnumerable<ResponseLogResp>> GetAllAsync();
    }
}