namespace TM.Core.Entities
{
    public class IssueUser
    {

        public int IssueId { get; set; }
        public Guid UserId { get; set; }

        public virtual Issue Issue { get; set; }
    }
}
