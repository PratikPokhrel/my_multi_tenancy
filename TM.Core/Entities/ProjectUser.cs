namespace TM.Core.Entities
{
    public class ProjectUser
    {
        public int ProjecId { get; set; }
        public Guid UserId { get; set; }
        public virtual Project  Project{ get; set; }
    }
}
