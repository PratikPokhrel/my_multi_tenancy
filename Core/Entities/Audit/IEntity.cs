using System;

namespace Core.Entities.Audit
{
    public interface IEntity<T>
    {
         T Id { get; set; }
    }
}