using System;

namespace Core.Entities.Audit
{
    public interface IHasCreator
    {
         Guid CreatedBy { get; set; }
    }
}