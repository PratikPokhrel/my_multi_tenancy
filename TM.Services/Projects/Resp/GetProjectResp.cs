using TM.Core.Enums;
using TM.Services.Issues.Resp;
using TM.Services.Projects.Rqst;

namespace TM.Services.Projects.Resp
{
    public class GetProjectResp: ProjectRqstResp
    {
        public GetProjectResp()
        {
            Issue = new List<IssueResp>();
        }
        public int Id { get; set; }
        public List<IssueResp> Issue { get; set; }
    }
}
