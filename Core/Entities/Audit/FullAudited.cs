using System;

namespace Core.Entities.Audit
{
    public abstract class FullAudited<T> : IFullAudited<T>
    {
        public T Id { get ; set ; }
        public Guid CreatedBy { get ; set ; }
        public DateTime CreatedOn { get ; set ; }
        public Guid? ModifiedBy { get ; set ; }
        public DateTime? ModifiedOn { get ; set ; }
        public Guid? DeletedBy { get ; set ; }
        public DateTime? DeletedOn { get ; set ; }
        public bool IsDeleted { get; set; }
    }
}