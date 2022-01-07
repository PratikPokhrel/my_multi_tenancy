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
				UserId = _clientInfoProvider.UserId,
				TenantId = _tenantProvider.TenantId,
				TenantName = _tenantProvider.Tenant.Identifier,
				ActionName = actionName,
				ApiName = apiName
			};
			await _repository.AddAsync(responseLogs).ConfigureAwait(false);
			await _unitOfWork.CommitAsync().ConfigureAwait(false);
		}

		public async Task<IEnumerable<ResponseLogResp>> GetAllAsync()
		{
			var responseLogs=await _repository.GetAllAsync().ConfigureAwait(false);
			return responseLogs.MapToResp();
		}
	}

	public  class ResponseLogResp
	{
		/// <summary>
		/// Response
		/// </summary>
		public string? Response { get; set; }
		/// <summary>
		/// Tenant name
		/// </summary>
		public string? TenantName { get; set; }
		/// <summary>
		/// Action name
		/// </summary>
		public string? ActionName { get; set; }
		public Guid TenantId { get; set; }
		public Guid? UserId { get; set; }
		/// <summary>
		/// Api name
		/// </summary>
		public string? ApiName { get; set; }

	   
	}

	public static class Mapper
	{
		public static IEnumerable<ResponseLogResp> MapToResp(this IEnumerable<ResponseLogs> logs)
		{
			return logs.Select(e => new ResponseLogResp
			{
				ActionName = e.ActionName,
				ApiName = e.ApiName,
				Response = e.Response,
				TenantName = e.TenantName,
				UserId = e.UserId,
				TenantId = e.TenantId
			});
		}
	}
}