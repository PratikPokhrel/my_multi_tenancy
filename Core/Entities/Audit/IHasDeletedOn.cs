using System;

namespace Core.Entities.Audit
{
    public interface IHasDeletedOn
    {
        DateTime?  DeletedOn{get;set;}
    }
}