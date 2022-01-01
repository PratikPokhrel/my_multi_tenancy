using System;

namespace Core.Entities.Audit
{
    public interface ISoftDeleted
    {
        bool IsDeleted{get;set;}
    }
}