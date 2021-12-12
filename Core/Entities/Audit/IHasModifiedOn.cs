using System;

namespace Core.Entities.Audit
{
    public interface IHasModifiedOn
    {
        DateTime? ModifiedOn{get;set;}
    }
}