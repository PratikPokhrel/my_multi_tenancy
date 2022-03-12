using Core.Entities.Audit;
using TM.Core.Enums;

namespace TM.Core.Entities
{
    public class Issue:FullAudited<int>
    {
        public Issue()
        {
            IssueUser = new HashSet<IssueUser>();
        }

        public string Title { get; set; }
        public IssueType Type{ get; set; }
        public IssueStatus Status{ get; set; }
        public IssuePriority Priority { get; set; }
        public int ListPosition { get; set; }
        public string Description { get; set; }
        public string DescriptionText { get; set; }
        public int? Estimate { get; set; }
        public int? TimeSpent { get; set; }
        public int? TimeRemaining { get; set; }
        public int ReporterId { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<IssueUser> IssueUser { get; set; }
    }
}
