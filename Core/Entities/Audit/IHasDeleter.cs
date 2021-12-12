using System;

namespace Core.Entities.Audit
{
    public interface IHasDeleter
    {
        Guid? DeletedBy{get;set;}
    }
}