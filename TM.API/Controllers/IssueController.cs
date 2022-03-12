using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TM.Core.Entities;
using TM.Services.Issues;
using TM.Services.Issues.Resp;
using TM.Services.Issues.Rqst;

namespace TM.API.Controllers
{
    //[Authorize]
    [Route("api/issues")]
    public class IssueController : Controller
    {
        private readonly IIssueService _issueService;

        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IssueResp>>> GetListAsync(int projectId,string searchText="")
        {
            var result=await _issueService.GetListAsync(projectId,searchText).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<IssueResp>>> AddAsync([FromBody]AddIssueRqst rqst)
        {
            var result=await _issueService.AddAsync(rqst).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result<IssueResp>>> UpdateAsync([FromBody] UpdateIssueRqst rqst)
        {
            var getByIdResp = await _issueService.FindByIdAsync(rqst.Id).ConfigureAwait(false);
            if (!getByIdResp.Succeeded)
                return Ok(getByIdResp);

            var result = await _issueService.UpdateAsync(rqst.ToDto(getByIdResp.Data)).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult<Result<IssueResp>>> DeleteAsync([FromQuery]int id)
        {
            var getByIdResp=await _issueService.FindByIdAsync(id).ConfigureAwait(false);
            if(!getByIdResp.Succeeded)
                return Ok(getByIdResp);

            var result = await _issueService.DeleteAsync(getByIdResp.Data).ConfigureAwait(false);
            return Ok(result);
        }
    }

    public static class Mapper
    {
        public static Issue ToDto(this UpdateIssueRqst rqst,Issue entity)
        {
           
            entity.Title=rqst.Title;
            entity.Description=rqst.Description; 
            entity.DescriptionText=rqst.DescriptionText;
            entity.Estimate=rqst.Estimate;
            entity.Status=rqst.Status;
            entity.TimeSpent=rqst.TimeSpent;
            entity.TimeRemaining=rqst.TimeRemaining;

            return entity;
        }
    }
}
