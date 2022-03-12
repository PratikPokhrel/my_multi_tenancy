using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TM.Services.Projects;
using TM.Services.Projects.Resp;
using TM.Services.Projects.Rqst;

namespace TM.API.Controllers
{
    [Route("api/projects")]
    public class ProjectController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<Result<GetProjectResp>>> GetAsync([FromServices]IProjectService service)
        {
            var result=await service.GetAsync().ConfigureAwait(false);
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<Result<GetProjectResp>>> AddAsync([FromServices] IProjectService service,[FromBody]AddProjectRqst rqst)
        {
            var result = await service.AddAsync(rqst).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
