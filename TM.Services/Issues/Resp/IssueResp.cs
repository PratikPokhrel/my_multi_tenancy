using TM.Core.Enums;
using TM.Services.Projects.Resp;

namespace TM.Services.Issues.Resp
{
    public class IssueResp
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IssueType Type { get; set; }
        public IssueStatus Status { get; set; }
        public IssuePriority Priority { get; set; }
        public int ListPosition { get; set; }
        public string Description { get; set; }
        public string DescriptionText { get; set; }
        public int? Estimate { get; set; }
        public int? TimeSpent { get; set; }
        public int? TimeRemaining { get; set; }
        public int ReporterId { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        public string CreatedByUser { get; set; }
        public string UpdatedByUser { get; set; }


        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

    }
}
