using Core.Entities.Audit;
using TM.Core.Enums;

namespace TM.Core.Entities
{
    public class Project:FullAudited<int>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public Project()
        {
            Issue = new HashSet<Issue>();
            ProjectUser=new HashSet<ProjectUser>();
        }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public ProjectCategory Category { get; set; }

        public virtual ICollection<Issue> Issue { get; set; }
        public virtual ICollection<ProjectUser> ProjectUser { get; set; }
    }
}
