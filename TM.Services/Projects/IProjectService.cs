using Core.Dto;
using TM.Services.Projects.Resp;
using TM.Services.Projects.Rqst;

namespace TM.Services.Projects
{
    public interface IProjectService
    {
        Task<Result<GetProjectResp>> GetAsync();
        Task<Result<GetProjectResp>> AddAsync(AddProjectRqst rqst);
    }
}
