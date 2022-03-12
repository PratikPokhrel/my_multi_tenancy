
using Core.Dto;
using Core.Infrastructure.Pagination;
using TM.Core.Entities;
using TM.Services.Issues.Resp;
using TM.Services.Issues.Rqst;

namespace TM.Services.Issues
{
    public interface IIssueService
    {
        Task<IEnumerable<IssueResp>> GetListAsync(int pzId,string searchText);
        Task<Result<Issue>> FindByIdAsync(int id);
        Task<Result<IssueResp>> AddAsync(AddIssueRqst rqst);
        Task<Result<IssueResp>> UpdateAsync(Issue rqst);
        Task<Result<IssueResp>> DeleteAsync(Issue issue);


    }
}
