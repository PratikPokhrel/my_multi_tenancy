using System;
using System.Text.Json.Serialization;

namespace Core.Entities.Audit
{
    public abstract class FullAudited<T> : IFullAudited<T>
    {
        public T Id { get ; set ; }
        [JsonIgnore]
        public Guid CreatedBy { get ; set ; }
        [JsonIgnore]
        public DateTime CreatedOn { get ; set ; }
        [JsonIgnore]
        public Guid? ModifiedBy { get ; set ; }
        [JsonIgnore]
        public DateTime? ModifiedOn { get ; set ; }
        [JsonIgnore]
        public Guid? DeletedBy { get ; set ; }
        [JsonIgnore]
        public DateTime? DeletedOn { get ; set ; }
        [JsonIgnore]
        public virtual bool IsDeleted { get; set; }
    }
}