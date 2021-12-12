using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Audit
{
    public class Entity<T> : IEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }
    }

}