using Core.Dto;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace my_multi_tenancy.Controllers
{
    [Route("api/response-logs")]
    public class ResponseLogsController : BaseApiController
    {
        #region "Ctor & properties"
        private readonly IResponseLogService _responseLogService;
        public ResponseLogsController(IResponseLogService responseLogService)
        {
            _responseLogService = responseLogService;
        }
        #endregion


        #region "Client API's"

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<ResponseLogResp>>>> GetAllAsync()
        {
            var result=await _responseLogService.GetAllAsync().ConfigureAwait(false);
            return AppOk(result);
        }

        #endregion
    }
}
