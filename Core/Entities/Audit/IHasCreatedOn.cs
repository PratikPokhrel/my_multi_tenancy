using System;

namespace Core.Entities.Audit
{
    public interface IHasCreatedOn
    {
        DateTime CreatedOn{get;set;}
    }
}