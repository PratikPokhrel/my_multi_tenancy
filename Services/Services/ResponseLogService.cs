using System.Threading.Tasks;
using Core.ClientInfo;
using Core.Entities;
using Core.Infrastructure.DataAccess;
using Core.Infrastructure.Tenancy;
using Newtonsoft.Json;

namespace Services.Services
{
    public class ResponseLogService:IResponseLogService
    {
        private readonly IRepository<ResponseLogs> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientInfoProvider _clientInfoProvider;
        private readonly ITenantProvider _tenantProvider;

        public ResponseLogService(IUnitOfWork unitOfWork,IClientInfoProvider clientInfoProvider,ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _clientInfoProvider = clientInfoProvider;
            _tenantProvider = tenantProvider;
            _repository = unitOfWork.GetRepository<ResponseLogs>();
        }

        public async Task AddAsync(object logs,string actionName,string apiName)
        {
            var responseLogs = new ResponseLogs()
            {
                Response = JsonConvert.SerializeObject(logs),
                userId = _clientInfoProvider.UserId,
                tenantId = _tenantProvider.TenantId,
                TenantName = _tenantProvider.Tenant.Identifier,
                ActionName = actionName,
                ApiName = apiName
            };
            await _repository.AddAsync(responseLogs).ConfigureAwait(false);
            await _unitOfWork.CommitAsync().ConfigureAwait(false);
        }
    }
}